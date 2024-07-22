using PET_ASP_.NET_Project.Models;

namespace PET_ASP_.NET_Project.Repositories
{
    public interface IProductRepository
    {
        public List<Product> GetProducts();

        public bool GetProduct(int id, out Product product);

        public void CreateProduct(string name, string description, decimal price, int stock);

        public bool UpdateProduct(int id, string name, string description, decimal price, int stock);

        public bool DeleteProduct(int id);
    }
}
