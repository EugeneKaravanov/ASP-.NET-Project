using Ecommerce;
using GatewayService.Models;
using ProductService.Models;
using ProductService;

namespace GatewayService.Utilities
{
    internal class Mapper
    {
        internal static ProductGRPC TransferProductDtoToProdutctGRPC(ProductDto productDto)
        {
            ProductGRPC productInfo = new ProductGRPC();

            productInfo.Name = productDto.Name;
            productInfo.Description = productDto.Description;
            productInfo.Price = Converter.ConvertDecimalToMoney(productDto.Price);
            productInfo.Stock = productDto.Stock;

            return productInfo;
        }

        internal static ProductWithIdDto TransferProductGRPCToProdutctWithIdDto(ProductGRPC productGrpc)
        {
            ProductWithIdDto productWithIdDto = new ProductWithIdDto();

            productWithIdDto.Id = productGrpc.Id;
            productWithIdDto.Name = productGrpc.Name;
            productWithIdDto.Description = productGrpc.Description;
            productWithIdDto.Price = Converter.ConvertMoneyToDecimal(productGrpc.Price);
            productWithIdDto.Stock = productGrpc.Stock;

            return productWithIdDto;
        }

        internal static ProductGRPC TransferProductDtoAndIdToProductGRPC(int id, ProductDto productDto)
        {
            ProductGRPC productGrpc = new ProductGRPC();

            productGrpc.Id = id;
            productGrpc.Name = productDto.Name;
            productGrpc.Description = productDto.Description;
            productGrpc.Price = Converter.ConvertDecimalToMoney(productDto.Price);
            productGrpc.Stock = productDto.Stock;

            return productGrpc;
        }

        internal static GetProductsRequest TransferGetProductsRequestDtoToGetProductsRequest(GetProductsRequestDto getProductsRequestDto)
        {
            GetProductsRequest request = new GetProductsRequest
            {
                ElementsOnPageCount = getProductsRequestDto.ElementsOnPageCount,
                ChoosenPageNumber = getProductsRequestDto.ChoosenPageNumber,
                NameFilter = getProductsRequestDto.NameFilter,
                MinPriceFilter = (uint?)getProductsRequestDto.MinPriceFilter,
                MaxPriceFilter = (uint?)getProductsRequestDto.MaxPriceFilter,
                SortArgument = getProductsRequestDto.SortArgument,
                IsReverseSort = getProductsRequestDto.IsReverseSort,
            };

            return request;
        }

        internal static PageDto<ProductWithIdDto> TransferPageGRPCToPageDto(PageGRPC pageGrpc)
        {
            PageDto<ProductWithIdDto> pageDto = new PageDto<ProductWithIdDto>();

            pageDto.TotalElementcCount = pageGrpc.TotalElementsCount;
            pageDto.TotalPagesCount = pageGrpc.TotalPagesCount;
            pageDto.ChoosenPageNumber = pageGrpc.ChoosenPageNumber;
            pageDto.ElementOnPageCount = pageGrpc.ElementsOnPageCount;
            pageDto.Products = new List<ProductWithIdDto>();
            
            foreach (ProductGRPC productWithIdGRPC in pageGrpc.Products)
                pageDto.Products.Add(TransferProductGRPCToProdutctWithIdDto(productWithIdGRPC));

            return pageDto;
        }
    }
}
