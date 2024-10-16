using ProductService.Models;
using Ecommerce;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        public Task<Page<ProductWithId>> GetProductsAsync(GetProductsRequest getProductsRequest, CancellationToken cancellationToken = default);

        public Task<ResultWithValue<Product>> GetProduct(int id, CancellationToken cancellationToken = default);

        public Task<Result> CreateProduct(Product product, CancellationToken cancellationToken = default);

        public Task<Result> UpdateProduct(int id, Product product, CancellationToken cancellationToken = default);

        public Task<Result> DeleteProduct(int id, CancellationToken cancellationToken = default);
    }
}
