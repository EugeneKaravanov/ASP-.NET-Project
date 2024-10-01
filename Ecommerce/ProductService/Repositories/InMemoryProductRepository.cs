using Ecommerce;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductService.Models;
using ProductService.Utilities;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace ProductService.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private volatile static int IdCounter = 0;
        private readonly ConcurrentDictionary<int, Product> _products = new ConcurrentDictionary<int, Product>();
        object locker = new object();

        public Page<ProductWithId> GetProducts(GetProductsRequest request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return null;

            int choosenPageNumber;
            Dictionary<int, Product> productsDictionary = new Dictionary<int, Product>();
            List<ProductWithId> products = new List<ProductWithId>();
            Page<ProductWithId> page = null;
            int totalElementsCount;
            int elementsOnPageCount;
            int totalPagesCount;
            bool isPageFormed = false;

            while (isPageFormed == false)
            {
                try
                {
                    int oldProductsRepistoryHash = _products.GetHashCode();

                    totalElementsCount = GetProductsCountAfterFiltration(request.NameFilter, request.MinPriceFilter, request.MaxPriceFilter);
                    elementsOnPageCount = request.ElementsOnPageCount > 0 ? request.ElementsOnPageCount : 1;
                    totalPagesCount = (int)Math.Ceiling(totalElementsCount / (double)elementsOnPageCount);

                    if (request.ChoosenPageNumber < 1)
                        choosenPageNumber = 1;
                    else if (request.ChoosenPageNumber > totalPagesCount)
                        choosenPageNumber = totalPagesCount;
                    else choosenPageNumber = request.ChoosenPageNumber;

                    products = _products
                        .Where(product => request.NameFilter == null || product.Value.Name.Contains(request.NameFilter))
                        .Where(product => request.MinPriceFilter.HasValue == false || product.Value.Price >= request.MinPriceFilter)
                        .Where(product => request.MaxPriceFilter.HasValue == false || product.Value.Price <= request.MaxPriceFilter)
                        .GetProductsAfterSorting(request.SortArgument, request.IsReverseSort)
                        .Skip(elementsOnPageCount * (choosenPageNumber - 1))
                        .Take(elementsOnPageCount)
                        .Select(product => Mapper.TansferProductAndIdToProductWithId(product.Key, product.Value))
                        .ToList();

                    if (oldProductsRepistoryHash == _products.GetHashCode())
                    {
                        page = new Page<ProductWithId>(totalElementsCount, totalPagesCount, choosenPageNumber, elementsOnPageCount, products);
                        isPageFormed = true;
                    }
                }
                catch
                {

                }
            }

            return page;
        }

        public bool GetProduct(int id, out Product product, CancellationToken cancellationToken)
        {
            product = null;

            if (cancellationToken.IsCancellationRequested)
                return false;

            bool isFinded = false;

            if (_products.TryGetValue(id, out Product value))
            {
                isFinded = true;
                product = value;
            }

            return isFinded;
        }

        public void CreateProduct(Product product, CancellationToken cancellationToken)
        {
            lock(locker)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                IdCounter++;
                _products.TryAdd(IdCounter, product);
            }
        }

        public bool UpdateProduct(int id, Product product, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            bool isUpdated = false;
            bool isCanExit = false;

            while (isCanExit == false)
            {
                if (_products.TryGetValue(id, out Product oldProduct) == false)
                {
                    isCanExit = true;
                    continue;
                }

                if (_products.TryUpdate(id, product, oldProduct))
                {
                    isUpdated = true;
                    isCanExit = true;
                }
            }

            return isUpdated;
        }

        public bool DeleteProduct(int id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            bool isDeleted = false;

             if (_products.TryRemove(id, out Product product))
                isDeleted = true;

            return isDeleted;
        }

        private int GetProductsCountAfterFiltration(string? nameFilter, uint? minPriceFilter, uint? maxPriceFilter)
        {
            int count = _products.
                Where(product => nameFilter == null || product.Value.Name.Contains(nameFilter)).
                Where(product => minPriceFilter.HasValue == false || product.Value.Price >= minPriceFilter).
                Where(product => maxPriceFilter.HasValue == false || product.Value.Price <= maxPriceFilter).
                Count();

            return count;
        }
    }
}
