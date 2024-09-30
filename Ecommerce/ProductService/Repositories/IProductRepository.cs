using ProductService.Models;
using Ecommerce;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        public Page<ProductWithId> GetProducts(GetProductsRequest getProductsRequest, CancellationToken cancellationToken);

        public bool GetProduct(int id, out Product product, CancellationToken cancellationToken);

        public void CreateProduct(Product product, CancellationToken cancellationToken);

        public bool UpdateProduct(int id, Product product, CancellationToken cancellationToken);

        public bool DeleteProduct(int id, CancellationToken cancellationToken);
    }
}
