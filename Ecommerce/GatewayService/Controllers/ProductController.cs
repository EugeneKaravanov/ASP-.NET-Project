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
        public async Task<GetProductsWithPaginationResponseDto> GetProducts(GetProductsWithPaginationRequestDto getProductsWithPaginationRequestDto)
        {
            GetProductsWithPaginationRequest request = new GetProductsWithPaginationRequest { ElementsOnPageCount = getProductsWithPaginationRequestDto.ElementsOnCurrentPageCount, CurrentPageNumber = getProductsWithPaginationRequestDto.CurrentPageNumber };
            GetProductsWithPaginationResponse response = await _productServiceClient.GetProductsAsync(request);
            GetProductsWithPaginationResponseDto result = new GetProductsWithPaginationResponseDto();

            List<ProductWithIdDto> products = new List<ProductWithIdDto>();
            List<ProductInfoWithID> productsResponse = response.Products.ToList<ProductInfoWithID>();

            foreach (var product in productsResponse)
            {
                ProductWithIdDto productWithIdDto = Mapper.TransferProductInfoWithIdToProdutctWithIdDto(product);
                products.Add(productWithIdDto);
            }

            result.elementsCount = response.AllElementsCount;
            result.currentPageNumber = response.CurrentPageNumber;
            result.elementsOnCurrentPageCount = response.ElementsOnCurentPageCount;
            result.products = products;

            return result;
        }

        [HttpGet("products/{id:int}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            GetProductRequest request = new GetProductRequest { Id = id };
            GetProductResponse response = await _productServiceClient.GetProductAsync(request);

            if (response.ResultCase == GetProductResponse.ResultOneofCase.Found)
            {
                ProductDto result = Mapper.TransferProductToProdutctInfo(response.Found.Product);

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
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            ProductInfo productInfo = Mapper.TransferProductToProdutctInfo(productDto);

            CreateProductRequest request = new CreateProductRequest { Product = productInfo };
            OperationStatusResponse response = await _productServiceClient.CreateProductAsync(request);

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
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            ProductInfo productInfo = Mapper.TransferProductToProdutctInfo(productDto);

            UpdateProductRequest request = new UpdateProductRequest { Id = id, Product = productInfo };
            OperationStatusResponse response = await _productServiceClient.UpdateProductAsync(request);

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
        public async Task<IActionResult> DeleteProduct(int id)
        {
            DeleteProductRequest request = new DeleteProductRequest { Id = id };
            OperationStatusResponse response = await _productServiceClient.DeleteProductAsync(request);

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

        [HttpGet("products/sort")]
        [ProducesResponseType(typeof(List<ProductWithIdDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSortedProducts(SortRequestDto sortRequestDto)
        {
            List<ProductWithIdDto> products = new List<ProductWithIdDto>();

            GetSortedProductsRequest request = new GetSortedProductsRequest { Argument = sortRequestDto.SortArgument, IsRevese = sortRequestDto.IsReverse };
            GetSortedProductsResponse response = await _productServiceClient.GetSortedProductsAsync(request);

            if (response.ResultCase == GetSortedProductsResponse.ResultOneofCase.FailureSort)
            {
                return BadRequest(response.FailureSort.Message);
            }

            foreach (var product in response.SuccessSort.Products)
            {
                ProductWithIdDto productWithIdDto = Mapper.TransferProductInfoWithIdToProdutctWithIdDto(product);

                products.Add(productWithIdDto);
            }

            return Ok(products);
        }

        [HttpGet("products/filter")]
        public async Task<List<ProductWithIdDto>> GetFiltredProducts(FilterRequestDto filterRequestDto)
        {
            FilterProductsRequest request = new FilterProductsRequest { Name = filterRequestDto.Name, MinPrice = (uint?)filterRequestDto.MinPrice, MaxPrice = (uint?)filterRequestDto.MaxPrice };
            GetProductsResponse response = await _productServiceClient.GetFiltredProductsAsync(request);

            List<ProductWithIdDto> products = new List<ProductWithIdDto>();
            List<ProductInfoWithID> productsResponse = response.Products.ToList<ProductInfoWithID>();

            foreach (var product in productsResponse)
            {
                ProductWithIdDto productWithIdDto = Mapper.TransferProductInfoWithIdToProdutctWithIdDto(product);
                products.Add(productWithIdDto);
            }

            return products;
        }
    }
}
