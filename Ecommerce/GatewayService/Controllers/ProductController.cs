using Microsoft.AspNetCore.Mvc;
using Ecommerce;
using Google.Type;
using GatewayService.Filters;
using GatewayService.Models;
using ProductService.Models;

namespace GatewayService.Controllers
{
    public class ProductController : Controller
    {
        private readonly Ecommerce.ProductService.ProductServiceClient _productServiceClient;

        public ProductController(Ecommerce.ProductService.ProductServiceClient productServiceClient) 
        {
            _productServiceClient = productServiceClient;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var request = new Google.Protobuf.WellKnownTypes.Empty();
            var response = await _productServiceClient.GetProductsAsync(request);
            List<ProductWithIdDto> products = new List<ProductWithIdDto>();
            List<ProductInfoWithID> productsResponse = response.Products.ToList<ProductInfoWithID>();

            foreach (var product in productsResponse)
            {
                ProductWithIdDto productWithIdDto = new ProductWithIdDto();
                ProductDto productDto = new ProductDto();

                productWithIdDto.Id = product.Id;
                
                productDto.Name = product.Product.Name;
                productDto.Description = product.Product.Description;
                productDto.Price = ConvertMoneyToDecimal(product.Product.Price);
                productDto.Stock = product.Product.Stock;

                productWithIdDto.Product = productDto;
                products.Add(productWithIdDto);
            }

            return Ok(products);
        }

        [HttpGet("products/{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var request = new GetProductRequest { Id = id };
            var response = await _productServiceClient.GetProductAsync(request);

            if (response.ResultCase == GetProductResponse.ResultOneofCase.Found)
            {
                ProductDto result = new ProductDto();
                result.Name = response.Found.Product.Name;
                result.Description = response.Found.Product.Description;
                result.Price = ConvertMoneyToDecimal(response.Found.Product.Price);
                result.Stock = response.Found.Product.Stock;
                return Ok(result);
            }
            else
            {
                OperationStatusDto result = new OperationStatusDto();
                result.Status = StatusDto.FAILURE;
                result.Message = response.NotFound.Message;
                return NotFound(result);
            }
        }

        [HttpPost("products")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            ProductInfo productInfo = new ProductInfo();
            OperationStatusDto result = new OperationStatusDto();

            productInfo.Name = productDto.Name;
            productInfo.Description = productDto.Description;
            productInfo.Price = ConvertDecimalToMoney(productDto.Price);
            productInfo.Stock = productDto.Stock;

            var request = new CreateProductRequest { Product = productInfo };
            var response = await _productServiceClient.CreateProductAsync(request);

            result.Message = response.Message;

            if (response.Status == Status.Success)
            {
                if (response.Status == Status.Success)
                    result.Status = StatusDto.SUCCESS;

                return Ok(result);
            }
            else
            {
                if (response.Status == Status.Failure)
                    result.Status = StatusDto.FAILURE;

                return BadRequest(result);
            }
        }

        [HttpPut("products/{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            ProductInfo productInfo = new ProductInfo();
            OperationStatusDto result = new OperationStatusDto();

            productInfo.Name = productDto.Name;
            productInfo.Description = productDto.Description;
            productInfo.Price = ConvertDecimalToMoney(productDto.Price);
            productInfo.Stock = productDto.Stock;

            var request = new UpdateProductRequest { Id = id, Product = productInfo };
            var response = await _productServiceClient.UpdateProductAsync(request);

            result.Message = response.Message;

            if (response.Status == Status.Success)
            {
                if (response.Status == Status.Success)
                    result.Status = StatusDto.SUCCESS;

                return Ok(result);
            }
            else
            {
                if (response.Status == Status.Failure)
                    result.Status = StatusDto.FAILURE;

                return BadRequest(result);
            }
        }

        [HttpDelete("products/{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            OperationStatusDto result = new OperationStatusDto();

            var request = new DeleteProductRequest { Id = id };
            var response = await _productServiceClient.DeleteProductAsync(request);

            result.Message = response.Message;

            if (response.Status == Status.Success)
            {
                if (response.Status == Status.Success)
                    result.Status = StatusDto.SUCCESS;

                return Ok(result);
            }
            else
            {
                if (response.Status == Status.Failure)
                    result.Status = StatusDto.FAILURE;

                return BadRequest(result);
            }
        }

        [HttpPost("products/sort")]
        public async Task<IActionResult> SortProducts(SortRequestDto sortRequestDto)
        {
            OperationStatusDto result = new OperationStatusDto();

            var request = new SortRequest { Argument = sortRequestDto.SortArgument, IsRevese = sortRequestDto.IsReverse };
            var response = await _productServiceClient.SortProductsAsync(request);

            result.Message = response.Message;

            if (response.Status == Status.Success)
            {
                if (response.Status == Status.Success)
                    result.Status = StatusDto.SUCCESS;

                return Ok(result);
            }
            else
            {
                if (response.Status == Status.Failure)
                    result.Status = StatusDto.FAILURE;

                return BadRequest(result);
            }
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
