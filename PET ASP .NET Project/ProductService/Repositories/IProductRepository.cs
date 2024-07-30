using ProductService.Models;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetProducts();

        public bool GetProduct(int id, out Product product);

        public bool CreateProduct(string name, string description, decimal price, int stock);

        public bool UpdateProduct(int id, string name, string description, decimal price, int stock);

        public bool DeleteProduct(int id);
    }
}
