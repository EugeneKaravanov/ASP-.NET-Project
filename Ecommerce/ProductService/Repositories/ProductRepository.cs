using Ecommerce;
using ProductService.Models;
using Dapper;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Transactions;
using ProductService.Utilities;

namespace ProductService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Page<ProductWithId>> GetProductsAsync(GetProductsRequest request, CancellationToken cancellationToken = default)
        {
            return null;
        }

        public async Task<ResultWithValue<ProductWithId>> GetProduct(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ResultWithValue<ProductWithId> result = new ResultWithValue<ProductWithId>();
            ProductWithId productWithId = null;
            string sqlString = $"SELECT 1 FROM Products WHERE id = @Id";
            using var conection = new NpgsqlConnection(_connectionString);

            await conection.OpenAsync(cancellationToken);
            productWithId = await conection.QuerySingleOrDefaultAsync<ProductWithId>(sqlString, new {Id = id});

            if (productWithId != null)
            {
                result.Status = Models.Status.Success;
                result.Value = productWithId;

                return result;
            }
            else
            {
                result.Status = Models.Status.NotFound;
                result.Message = $"Продукт c ID {id} отсутствует в базе данных!";

                return result;
            }
        }

        public async Task<Result> CreateProduct(Product product, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Result result = new Result();
            string sqlString = @"
                                WITH insert_result AS 
                                (
                                    INSERT INTO Products (name, description, price, stock)
                                    VALUES (@Name, @Description, @Price, @Stock)
                                    ON CONFLICT (name) DO NOTHING
                                    RETURNING id
                                )
                                SELECT id FROM insert_result";

            using var conection = new NpgsqlConnection(_connectionString);

            await conection.OpenAsync(cancellationToken);
            int? insertId = await conection.QuerySingleOrDefaultAsync<int?>(sqlString, product);

            if (insertId != null)
            {
                result.Status = Models.Status.Success;
                result.Message = "Продукт успешно добавлен!";

                return result;
            }
            else
            {
                result.Status = Models.Status.Failure;
                result.Message = "Не удалось добавить продукт, так как его имя уже используется!";

                return result;
            }
        }

        public async Task<Result> UpdateProduct(int id, Product product, CancellationToken cancellationToken = default)
        {
            Result result = new Result();
            ProductWithId productWithId = Mapper.TansferProductAndIdToProductWithId(id, product);
            string sqlString = @"WITH update_result AS
                                    (
                                    UPDATE Products SET
                                        Name = @Name,
                                        Description = @Description,
                                        Price = @Price,
                                        Stock = @Stock
                                        WHERE Id = @Id
                                        RETURNING Id
                                     )
                                SELECT CASE 
                                    WHEN EXISTS (SELECT 1 FROM update_result) THEN 'SUCCESS'
                                END;";

            using var conection = new NpgsqlConnection(_connectionString);
            await conection.OpenAsync(cancellationToken);

            try
            {
                string? updateStatus = await conection.QuerySingleOrDefaultAsync<string?>(sqlString, productWithId);

                if (updateStatus == "SUCCESS")
                {
                    result.Status = Models.Status.Success;
                    result.Message = "Продукт успешно добавлен!";

                    return result;
                }
                else
                {
                    result.Status = Models.Status.NotFound;
                    result.Message = $"Продукт c ID {id} отсутствует в базе данных!";

                    return result;
                }
            }
            catch
            {
                result.Status = Models.Status.Failure;
                result.Message = "Не удалось обновить продукт, так как его имя уже используется!";

                return result;
            }
        }

        public async Task<Result> DeleteProduct(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Result result = new Result();
            string sqlString = @"
                                WITH delete_result AS 
                                (
                                    DELETE FROM Products
                                    WHERE @Id = Id
                                    RETURNING Id
                                )
                                SELECT id FROM delete_result";

            using var conection = new NpgsqlConnection(_connectionString);

            await conection.OpenAsync(cancellationToken);
            int? deleteId = await conection.QuerySingleOrDefaultAsync<int?>(sqlString, new {Id = id});

            if (deleteId != null)
            {
                result.Status = Models.Status.Success;
                result.Message = "Продукт успешно удален!";

                return result;
            }
            else
            {
                result.Status = Models.Status.Failure;
                result.Message = $"Не удалось удалить продукт, так как продукта с ID {id} несуществует!";

                return result;
            }
        }
    }
}
