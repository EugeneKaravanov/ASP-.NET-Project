using Microsoft.AspNetCore.Mvc;
using Ecommerce;
using GatewayService.Models;
using GatewayService.Utilities;
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
        public async Task<PageDto<ProductWithIdDto>> GetProducts(Models.GetProductsRequestDto getProductsRequestDto, CancellationToken cancellationToken)
        {
            GetProductsRequest request = Mapper.TransferGetProductsRequestDtoToGetProductsRequest(getProductsRequestDto);
            GetProductsResponse response = await _productServiceClient.GetProductsAsync(request, cancellationToken: cancellationToken);

            PageDto<ProductWithIdDto> pageDto = Mapper.TransferPageGRPCToPageDto(response.Page);

            return pageDto;
        }

        [HttpGet("products/{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id, CancellationToken cancellationToken)
        {
            GetProductRequest request = new GetProductRequest { Id = id };
            GetProductResponse response = await _productServiceClient.GetProductAsync(request, cancellationToken: cancellationToken);

            if (response.ResultCase == GetProductResponse.ResultOneofCase.Found)
            {
                ProductDto result = Mapper.TransferProductGRPCToProductDto(response.Found.Product);

                return Ok(result);
            }
            else
            {
                string message = response.NotFound.Message;

                return NotFound(message);
            }
        }

        [HttpPost("products")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto, CancellationToken cancellationToken)
        {
            ProductGRPC productGRPC = Mapper.TransferProductDtoToProdutctGRPC(productDto);

            CreateProductRequest request = new CreateProductRequest { Product = productGRPC };
            OperationStatusResponse response = await _productServiceClient.CreateProductAsync(request, cancellationToken: cancellationToken);

            string message = response.Message;

            if (response.Status == Status.Success)
            {
                return Ok(message);
            }
            else
            {
                return BadRequest(message);
            }
        }

        [HttpPut("products/{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto, CancellationToken cancellationToken)
        {
            ProductWithIdGRPC productWithIdGRPC = Mapper.TransferProductDtoAndIdToProductWithIdGRPC(id, productDto);

            UpdateProductRequest request = new UpdateProductRequest { Product = productWithIdGRPC };
            OperationStatusResponse response = await _productServiceClient.UpdateProductAsync(request, cancellationToken: cancellationToken);

            string message = response.Message;

            if (response.Status == Status.Success)
            {
                return Ok(message);
            }
            else
            {
                return BadRequest(message);
            }
        }

        [HttpDelete("products/{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            DeleteProductRequest request = new DeleteProductRequest { Id = id };
            OperationStatusResponse response = await _productServiceClient.DeleteProductAsync(request, cancellationToken: cancellationToken);

            string message = response.Message;

            if (response.Status == Status.Success)
            {
                return Ok(message);
            }
            else
            {
                return BadRequest(message);
            }
        }
    }
}
