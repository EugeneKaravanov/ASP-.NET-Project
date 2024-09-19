using Ecommerce;
using ProductService.Models;

namespace ProductService.Utilities
{
    internal class Mapper
    {
        internal static ProductInfo TransferProductToProdutctInfo(Product product)
        {
            ProductInfo productInfo = new ProductInfo();

            productInfo.Name = product.Name;
            productInfo.Description = product.Description;
            productInfo.Price = Converter.ConvertDecimalToMoney(product.Price);
            productInfo.Stock = product.Stock;

            return productInfo;
        }

        internal static Product TransferProductInfoToProduct(ProductInfo productInfo)
        {
            Product product = new Product();

            product.Name = productInfo.Name;
            product.Description = productInfo.Description;
            product.Price = Converter.ConvertMoneyToDecimal(productInfo.Price);
            product.Stock = productInfo.Stock;

            return product;
        }

        internal static ProductInfoWithID TransferProductAndIdToProductInfoWithId(int id, Product product)
        {
            ProductInfoWithID productInfoWithID = new ProductInfoWithID();

            productInfoWithID.Id = id;
            productInfoWithID.Name = product.Name;
            productInfoWithID.Description = product.Description;
            productInfoWithID.Price = Converter.ConvertDecimalToMoney(product.Price);
            productInfoWithID.Stock = product.Stock;

            return productInfoWithID;
        }
    }
}
