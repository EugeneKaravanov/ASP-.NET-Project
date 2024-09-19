using Ecommerce;
using GatewayService.Models;
using ProductService.Models;

namespace GatewayService.Utilities
{
    internal class Mapper
    {
        internal static ProductDto TransferProductToProdutctInfo(ProductInfo productInfo)
        {
            ProductDto product = new ProductDto();

            product.Name = productInfo.Name;
            product.Description = productInfo.Description;
            product.Price = Converter.ConvertMoneyToDecimal(productInfo.Price);
            product.Stock = productInfo.Stock;

            return product;
        }

        internal static ProductInfo TransferProductToProdutctInfo(ProductDto productDto)
        {
            ProductInfo productInfo = new ProductInfo();

            productInfo.Name = productDto.Name;
            productInfo.Description = productDto.Description;
            productInfo.Price = Converter.ConvertDecimalToMoney(productDto.Price);
            productInfo.Stock = productDto.Stock;

            return productInfo;
        }

        internal static ProductWithIdDto TransferProductInfoWithIdToProdutctWithIdDto(ProductInfoWithID productInfoWithID)
        {
            ProductWithIdDto productWithIdDto = new ProductWithIdDto();

            productWithIdDto.Id = productInfoWithID.Id;
            productWithIdDto.Name = productInfoWithID.Name;
            productWithIdDto.Description = productInfoWithID.Description;
            productWithIdDto.Price = Converter.ConvertMoneyToDecimal(productInfoWithID.Price);
            productWithIdDto.Stock = productInfoWithID.Stock;

            return productWithIdDto;
        }
    }
}
