using ProductService.Models;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        public Dictionary<int, Product> GetProducts();

        public bool GetProduct(int id, out Product product);

        public void CreateProduct(Product product);

        public bool UpdateProduct(int id, Product product);

        public bool DeleteProduct(int id);
    }
}
