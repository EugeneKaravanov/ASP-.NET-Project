using ProductService.Models;
using Ecommerce;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        public List<ProductInfoWithID> GetProducts();

        public bool GetProduct(int id, out Product product);

        public void CreateProduct(Product product);

        public bool UpdateProduct(int id, Product product);

        public bool DeleteProduct(int id);
    }
}
