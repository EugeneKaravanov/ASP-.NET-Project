using Ecommerce;
using ProductService.Models;
using ProductService.Utilities;
using System.Collections.Concurrent;

namespace ProductService.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private static int IdCounter = 0;
        private readonly Dictionary<int, Product> _products = new Dictionary<int, Product>();

        public List<ProductInfoWithID> GetProducts()
        {
            List<ProductInfoWithID> sendingProducts = new List<ProductInfoWithID>();

            foreach (var product in _products)
                sendingProducts.Add(Mapper.TransferProductAndIdToProductInfoWithId(product.Key, product.Value));

            return sendingProducts;
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
    }
}
