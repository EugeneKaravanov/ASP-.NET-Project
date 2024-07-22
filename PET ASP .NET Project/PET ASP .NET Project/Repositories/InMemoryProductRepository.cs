using PET_ASP_.NET_Project.Models;
using System.Runtime.CompilerServices;

namespace PET_ASP_.NET_Project.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private List<Product> _products = new List<Product>();

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

            for (int i = 0; i < _products.Count; i++)
                if (_products[i].Id == id)
                {
                    product = _products[i];
                    isFinded = true;
                }

            return isFinded;
        }

        public void CreateProduct(string name, string description, decimal price, int stock)
        {
            _products.Add(new Product(name, description, price, stock));
        }

        public bool UpdateProduct(int id, string name, string description, decimal price, int stock)
        {
            bool isUpdated = false;

            for (int i = 0; i < _products.Count; i++)
                if (_products[i].Id == id)
                {
                    _products[i].Name = name;
                    _products[i].Description = description;
                    _products[i].Price = price;
                    _products[i].Stock = stock;
                    isUpdated = true;
                }

            return isUpdated;
        }

        public bool DeleteProduct(int id)
        {
            bool isDeleted = false;

            for (int i = 0; i < _products.Count; i++)
                if (_products[i].Id == id)
                {
                    _products.RemoveAt(i);
                    isDeleted = true;
                }

            return isDeleted;
        }
    }
}
