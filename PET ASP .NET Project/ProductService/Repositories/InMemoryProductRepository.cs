using ProductService.Models;
using ProductService.Validators;

namespace ProductService.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private static int IdCounter = 0;
        private Dictionary<int, Product> _products = new Dictionary<int, Product>();
        private ProductValidator _productValidator;

        public InMemoryProductRepository(ProductValidator productValidator) 
        {
            _productValidator = productValidator;
        }

        public List<Product> GetProducts()
        {
            List<Product> sendingProducts = new List<Product>();

            for (int i = 0; i < _products.Count; i++)
                sendingProducts.Add(_products[i]);

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

        public bool CreateProduct(string name, string description, decimal price, int stock)
        {
            bool isAdded = false;
            Product addingProduct = new Product(name, description, price, stock);

            if(_productValidator.Validate(addingProduct).IsValid)
            {
                _products.Add(IdCounter++, addingProduct);
                isAdded = true;
            }

            return isAdded;
        }

        public bool UpdateProduct(int id, string name, string description, decimal price, int stock)
        {
            bool isUpdated = false;
            Product changingProduct = new Product(name, description, price, stock);

            if (_products.ContainsKey(id))
                if (_productValidator.Validate(changingProduct).IsValid)
                {
                    _products[id] = changingProduct;
                    isUpdated = true;
                }

            return isUpdated;
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
