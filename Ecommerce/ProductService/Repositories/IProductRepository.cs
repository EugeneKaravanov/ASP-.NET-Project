using ProductService.Models;
using Ecommerce;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        public Page<ProductWithId> GetProducts(GetProductsRequest getProductsRequest, CancellationToken cancellationToken = default);

        public ResultWithValue<Product> GetProduct(int id, CancellationToken cancellationToken = default);

        public Result CreateProduct(Product product, CancellationToken cancellationToken = default);

        public Result UpdateProduct(int id, Product product, CancellationToken cancellationToken = default);

        public Result DeleteProduct(int id, CancellationToken cancellationToken = default);
    }
}
