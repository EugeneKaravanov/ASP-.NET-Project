using ProductService.Models;
using ProductService.Validators;

namespace ProductService.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private static int IdCounter = 0;
        private Dictionary<int, Product> _products = new Dictionary<int, Product>() { };
        private ProductValidator _productValidator;

        public InMemoryProductRepository(ProductValidator productValidator) 
        {
            _productValidator = productValidator;
        }

        public Dictionary<int, Product> GetProducts()
        {
            Dictionary<int, Product> sendingProducts = new Dictionary<int, Product>();

            foreach (var product in _products)
                sendingProducts.Add(product.Key, product.Value);

            return sendingProducts;
        }

        public bool GetProduct(int id, out Product product)
        {
            product = null;
            bool isFinded = false;

            if (_products.ContainsKey(id))
            {
                isFinded = true;
                product = _products[id];
            }

            return isFinded;
        }

        public void CreateProduct(Product product)
        {
            _products.Add(++IdCounter, product);
        }

        public bool UpdateProduct(int id, Product product)
        {
            if (_products.ContainsKey(id))
            {
                _products[id] = product;
                return true;
            }

            return false;
        }

        public bool DeleteProduct(int id)
        {
            bool isDeleted = false;

            if (_products.ContainsKey(id))
            {
                _products.Remove(id);
                isDeleted = true;
            }

            return isDeleted;
        }
    }
}
