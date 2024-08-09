using Microsoft.AspNetCore.Mvc;
using static Ecommerce.ProductService;
using Ecommerce;
using Google.Type;
using GatewayService.Filters;

namespace GatewayService.Controllers
{
    [ServiceFilter(typeof(CustomHeaderFilter))]
    public class ProductController : Controller
    {
        private readonly Ecommerce.ProductService.ProductServiceClient _productServiceClient;

        public ProductController(Ecommerce.ProductService.ProductServiceClient productServiceClient) 
        {
            _productServiceClient = productServiceClient;
        }

        [HttpGet("products")]
        public async Task<List<ProductInfoWithID>> GetProducts()
        {
            var request = new Google.Protobuf.WellKnownTypes.Empty();
            var response = await _productServiceClient.GetProductsAsync(request);
            List<ProductInfoWithID> products = response.Products.ToList<ProductInfoWithID>();

            return products;
        }

        [HttpGet("products/{id:int}")]
        public async Task<GetProductResponse> GetProduct([FromQuery] int id)
        {
            var request = new GetProductRequest { Id = id };
            var response = await _productServiceClient.GetProductAsync(request);
            GetProductResponse result = new GetProductResponse();

            result.Status = response.Status;

            if (response.ResultCase == GetProductResponse.ResultOneofCase.Found)
            {
                result.Found = response.Found;
            }
            else
            {
                result.NotFound = response.NotFound;
            }

            return result;
        }

        [HttpPost("products")]
        public async Task<OperationStatusResponse> CreateProduct([FromBody] string? name, [FromBody] string? description, [FromBody] decimal? price, [FromBody] int? stock)
        {
            ProductInfo productInfo = new ProductInfo();

            productInfo.Name = name;
            productInfo.Description = description;
            productInfo.Price = ConvertDecimalToMoney(price.GetValueOrDefault());
            productInfo.Stock = stock.GetValueOrDefault();

            var request = new CreateProductRequest { Product = productInfo };
            var response = await _productServiceClient.CreateProductAsync(request);

            return response;
        }

        [HttpPut("products/{id:int}")]
        public async Task<OperationStatusResponse> UpdateProduct(int id, [FromBody] string? name, [FromBody] string? description, [FromBody] decimal? price, [FromBody] int? stock)
        {
            ProductInfo productInfo = new ProductInfo();

            productInfo.Name = name;
            productInfo.Description = description;
            productInfo.Price = ConvertDecimalToMoney(price.GetValueOrDefault());
            productInfo.Stock = stock.GetValueOrDefault();

            var request = new UpdateProductRequest { Id = id, Product = productInfo };
            var response = await _productServiceClient.UpdateProductAsync(request);

            return response;
        }

        [HttpDelete("products/{id:int}")]
        public async Task<OperationStatusResponse> DeleteProduct(int id)
        {
            var request = new DeleteProductRequest { Id = id };
            var response = await _productServiceClient.DeleteProductAsync(request);

            return response;
        }

        private static Money ConvertDecimalToMoney(decimal value)
        {
            Money money = new Money();

            money.Units = (long)value;
            money.Nanos = (int)(value % 1);
            money.CurrencyCode = "RUB";

            return money;
        }

        private static decimal ConvertMoneyToDecimal(Money money)
        {
            decimal price = money.Units + money.Nanos;

            return price;
        }
    }
}
