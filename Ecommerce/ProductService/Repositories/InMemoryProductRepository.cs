using Ecommerce;
using ProductService.Models;
using ProductService.Utilities;

namespace ProductService.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private static int IdCounter = 0;
        private readonly Dictionary<int, Product> _products = new Dictionary<int, Product>();

        public Page<ProductWithId> GetProducts(GetProductsRequest request)
        {
            int totalElementsCount = GetProductsCountAfterFiltration(request.NameFilter, request.MinPriceFilter, request.MaxPriceFilter);
            int elementsOnPageCount = request.ElementsOnPageCount > 0 ? request.ElementsOnPageCount : 1;
            int totalPagesCount = totalElementsCount % elementsOnPageCount == 0 ? totalElementsCount / elementsOnPageCount : totalElementsCount / elementsOnPageCount + 1;
            int choosenPageNumber;
            IEnumerable<KeyValuePair<int, Product>> productsRequest;
            Dictionary<int, Product> productsDictionary = new Dictionary<int, Product>();
            List<ProductWithId> products = new List<ProductWithId>();
            Page<ProductWithId> page;

            if (request.ChoosenPageNumber < 1)
                choosenPageNumber = 1;
            else if (request.ChoosenPageNumber > totalPagesCount)
                choosenPageNumber = totalPagesCount;
            else choosenPageNumber = request.ChoosenPageNumber;

            productsRequest = _products
                .Where(product => request.NameFilter == null || product.Value.Name.Contains(request.NameFilter))
                .Where(product => request.MinPriceFilter.HasValue == false || product.Value.Price >= request.MinPriceFilter)
                .Where(product => request.MaxPriceFilter.HasValue == false || product.Value.Price <= request.MaxPriceFilter);
            productsRequest = GetProductsAfterSorting(productsRequest, request.SortArgument, request.IsReverseSort);
            products = productsRequest
                .Skip(elementsOnPageCount * (choosenPageNumber - 1))
                .Take(elementsOnPageCount)
                .Select(product => Mapper.TansferProductAndIdToProductWithId(product.Key, product.Value))
                .ToList();
            page = new Page<ProductWithId>(totalElementsCount, totalPagesCount, choosenPageNumber, elementsOnPageCount, products);

            return page;
        }

        public bool GetProduct(int id, out Product product)
        {
            product = null;
            bool isFinded = false;

            if (_products.TryGetValue(id, out Product value))
            {
                isFinded = true;
                product = value;
            }

            return isFinded;
        }

        public void CreateProduct(Product product)
        {
            _products.Add(++IdCounter, product);
        }

        public bool UpdateProduct(int id, Product product)
        {
            bool isUpdated = false;

            if (_products.ContainsKey(id))
            {
                _products[id] = product;
                isUpdated = true;
            }

            return isUpdated;
        }

        public bool DeleteProduct(int id)
        {
            bool isDeleted = false;

            if (_products.Remove(id))
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

        private IEnumerable<KeyValuePair<int, Product>> GetProductsAfterSorting(IEnumerable<KeyValuePair<int, Product>> products, string sortArgument, bool isReverseSort)
        {
            if (sortArgument != "Name" && sortArgument != "Price")
            {
                return products;
            }

            switch (sortArgument)
            {
                case "Name":
                    if (isReverseSort == false)
                        products = products.OrderBy(product => product.Value.Name);
                    else
                        products = products.OrderByDescending(product => product.Value.Name);
                    break;

                case "Price":
                    if (isReverseSort == false)
                        products = products.OrderBy(product => product.Value.Price);
                    else
                        products = products.OrderByDescending(product => product.Value.Price);
                    break;
            }

            return products;
        }
    }
}
