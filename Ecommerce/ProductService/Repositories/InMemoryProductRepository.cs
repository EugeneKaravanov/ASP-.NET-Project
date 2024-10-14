using Ecommerce;
using ProductService.Models;
using ProductService.Utilities;
using System.Collections.Concurrent;

namespace ProductService.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private static int IdCounter = 0;
        private readonly ConcurrentDictionary<int, Product> _products = new ConcurrentDictionary<int, Product>();
        private readonly ConcurrentDictionary<string, Product> _usedNames = new ConcurrentDictionary<string, Product>();

        public Page<ProductWithId> GetProducts(GetProductsRequest request, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                Task.FromCanceled(cancellationToken);

            int chosenPageNumber;
            Dictionary<int, Product> productsDictionary = new Dictionary<int, Product>();
            List<ProductWithId> products = new List<ProductWithId>();
            Page<ProductWithId> page = null;
            int totalElementsCount = GetFiltredProductsCount(request.NameFilter, request.MinPriceFilter, request.MaxPriceFilter);
            int elementsOnPageCount = request.ElementsOnPageCount > 0 ? request.ElementsOnPageCount : 1;
            int totalPagesCount = (int)Math.Ceiling(totalElementsCount / (double)elementsOnPageCount);

            if (request.ChoosenPageNumber < 1)
                chosenPageNumber = 1;
            else if (request.ChoosenPageNumber > totalPagesCount)
                chosenPageNumber = totalPagesCount;
            else chosenPageNumber = request.ChoosenPageNumber;

            products = _products
                .Where(product => request.NameFilter == null || product.Value.Name.Contains(request.NameFilter))
                .Where(product => request.MinPriceFilter.HasValue == false || product.Value.Price >= request.MinPriceFilter)
                .Where(product => request.MaxPriceFilter.HasValue == false || product.Value.Price <= request.MaxPriceFilter)
                .GetProductsAfterSorting(request.SortArgument, request.IsReverseSort)
                .Skip(elementsOnPageCount * (chosenPageNumber - 1))
                .Take(elementsOnPageCount)
                .Select(product => Mapper.TansferProductAndIdToProductWithId(product.Key, product.Value))
                .ToList();

            page = new Page<ProductWithId>(totalElementsCount, totalPagesCount, chosenPageNumber, elementsOnPageCount, products);

            return page;
        }

        public ResultWithValue<Product> GetProduct(int id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                Task.FromCanceled(cancellationToken);

            ResultWithValue<Product> result = new ResultWithValue<Product>();

            if (_products.TryGetValue(id, out Product product))
            {
                result.Status = Models.Status.Success;
                result.Value = product;
            }
            else
            {
                result.Status = Models.Status.NotFound;
                result.Message = "Продукт отсутствует в базе данных!";
            }

            return result;
        }

        public Result CreateProduct(Product product, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                Task.FromCanceled(cancellationToken);

            Result result = new Result();
            int localCounter = Interlocked.Increment(ref IdCounter);

            if (_usedNames.TryAdd(product.Name, product))
            {
                _products.TryAdd(localCounter, product);
                result.Status = Models.Status.Success;
                result.Message = "Продукт успешно добавлен!";
            }
            else
            {
                result.Status = Models.Status.Failure;
                result.Message = "Не удалось добавить продукт, так как его имя уже используется!";
            }

            return result;
        }

        public Result UpdateProduct(int id, Product product, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                Task.FromCanceled(cancellationToken);

            Result result = new Result();
            bool isOperagtionComplited = false;

            while (isOperagtionComplited == false)
            {
                if (cancellationToken.IsCancellationRequested)
                    Task.FromCanceled(cancellationToken);

                if (_products.TryGetValue(id, out Product oldProduct) == false)
                {
                    isOperagtionComplited = true;
                    result.Status= Models.Status.NotFound;
                    result.Message = "Не удалсь обновить продукт, так как он отсутствует в базе данных!";

                    continue;
                }

                if (oldProduct.Name == product.Name && _products.TryUpdate(id, product, oldProduct))
                {
                    isOperagtionComplited = true;
                    result.Status = Models.Status.Success;
                    result.Message = "Продукт успешно обновлен!";

                    continue;
                }

                if (_usedNames.TryAdd(product.Name, product) == false)
                {
                    isOperagtionComplited = true;
                    result.Status = Models.Status.Failure;
                    result.Message = "Не удалось обновить продукт, так как его имя не уникально!";

                    continue;
                }

                if (_products.TryUpdate(id, product, oldProduct))
                {
                    isOperagtionComplited = true;
                    result.Status = Models.Status.Success;
                    result.Message = "Продукт успешно обновлен!";

                    continue;
                }
                else
                {
                    _usedNames.TryRemove(product.Name, out product);
                }
            }

            return result;
        }

        public Result DeleteProduct(int id, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                Task.FromCanceled(cancellationToken);

            Result result = new Result();

            if (_products.TryRemove(id, out Product product))
            {
                _usedNames.TryRemove(product.Name, out Product value);
                result.Status = Models.Status.Success;
                result.Message = "Продукт успешно удален!";
            }
            else
            {
                result.Status = Models.Status.NotFound;
                result.Message = $"Продукт с ID {id} отсутствует в базе данных!";
            }

            return result;
        }

        private int GetFiltredProductsCount(string? nameFilter, uint? minPriceFilter, uint? maxPriceFilter)
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
