using Ecommerce;
using GatewayService.Models;
using ProductService.Models;

namespace GatewayService.Utilities
{
    internal class Mapper
    {
        internal static ProductDto TransferProductGRPCToProductDto(ProductGRPC productGrpc)
        {
            ProductDto product = new ProductDto();

            product.Name = productGrpc.Name;
            product.Description = productGrpc.Description;
            product.Price = Converter.ConvertMoneyToDecimal(productGrpc.Price);
            product.Stock = productGrpc.Stock;

            return product;
        }

        internal static ProductGRPC TransferProductDtoToProdutctGRPC(ProductDto productDto)
        {
            ProductGRPC productInfo = new ProductGRPC();

            productInfo.Name = productDto.Name;
            productInfo.Description = productDto.Description;
            productInfo.Price = Converter.ConvertDecimalToMoney(productDto.Price);
            productInfo.Stock = productDto.Stock;

            return productInfo;
        }

        internal static ProductWithIdDto TransferProductWithIdGRPCToProdutctWithIdDto(ProductWithIdGRPC productWithIdGrpc)
        {
            ProductWithIdDto productWithIdDto = new ProductWithIdDto();

            productWithIdDto.Id = productWithIdGrpc.Id;
            productWithIdDto.Name = productWithIdGrpc.Name;
            productWithIdDto.Description = productWithIdGrpc.Description;
            productWithIdDto.Price = Converter.ConvertMoneyToDecimal(productWithIdGrpc.Price);
            productWithIdDto.Stock = productWithIdGrpc.Stock;

            return productWithIdDto;
        }

        internal static ProductWithIdGRPC TransferProductDtoAndIdToProductWithIdGRPC(int id, ProductDto productDto)
        {
            ProductWithIdGRPC productWithIdGRPC = new ProductWithIdGRPC();

            productWithIdGRPC.Id = id;
            productWithIdGRPC.Name = productDto.Name;
            productWithIdGRPC.Description = productDto.Description;
            productWithIdGRPC.Price = Converter.ConvertDecimalToMoney(productDto.Price);
            productDto.Stock = productWithIdGRPC.Stock;

            return productWithIdGRPC;
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
            
            foreach (ProductWithIdGRPC productWithIdGRPC in pageGrpc.Products)
                pageDto.Products.Add(TransferProductWithIdGRPCToProdutctWithIdDto(productWithIdGRPC));

            return pageDto;
        }
    }
}
