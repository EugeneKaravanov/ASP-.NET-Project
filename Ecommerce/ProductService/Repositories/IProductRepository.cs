using ProductService.Models;
using Ecommerce;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        public Page<ProductWithId> GetProducts(GetProductsRequest getProductsRequest);

        public bool GetProduct(int id, out Product product);

        public void CreateProduct(Product product);

        public bool UpdateProduct(int id, Product product);

        public bool DeleteProduct(int id);
    }
}
