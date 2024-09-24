using Ecommerce;
using ProductService.Models;

namespace ProductService.Utilities
{
    internal class Mapper
    {
        internal static ProductGRPC TransferProductToProductGrpc(Product product)
        {
            ProductGRPC productGrpc = new ProductGRPC();

            productGrpc.Name = product.Name;
            productGrpc.Description = product.Description;
            productGrpc.Price = Converter.ConvertDecimalToMoney(product.Price);
            productGrpc.Stock = product.Stock;

            return productGrpc;
        }

        internal static Product TransferProductGRPCToProduct(ProductGRPC productGrpc)
        {
            Product product = new Product();

            product.Name = productGrpc.Name;
            product.Description = productGrpc.Description;
            product.Price = Converter.ConvertMoneyToDecimal(productGrpc.Price);
            product.Stock = productGrpc.Stock;

            return product;
        }

        internal static ProductWithIdGRPC TransferProductAndIdToProductWithIdGRPC(int id, Product product)
        {
            ProductWithIdGRPC productWithIdGrpc = new ProductWithIdGRPC();

            productWithIdGrpc.Id = id;
            productWithIdGrpc.Name = product.Name;
            productWithIdGrpc.Description = product.Description;
            productWithIdGrpc.Price = Converter.ConvertDecimalToMoney(product.Price);
            productWithIdGrpc.Stock = product.Stock;

            return productWithIdGrpc;
        }

        internal static Product TransferProductWithIdGRPCToProductAndId(ProductWithIdGRPC productWithIdGRPC, out int id)
        {
            Product product = new Product();

            id = productWithIdGRPC.Id;
            product.Name = productWithIdGRPC.Name;
            product.Description = productWithIdGRPC.Description;
            product.Price = Converter.ConvertMoneyToDecimal(productWithIdGRPC.Price);
            product.Stock = productWithIdGRPC.Stock;

            return product;
        }

        internal static ProductWithIdGRPC TransferProductWithIdToProductAndIdGRPC(ProductWithId productWithId)
        {
            ProductWithIdGRPC productWithIdGRPC = new ProductWithIdGRPC();

            productWithIdGRPC.Id = productWithId.Id;
            productWithIdGRPC.Name = productWithId.Name;
            productWithIdGRPC.Description = productWithId.Description;
            productWithIdGRPC.Price = Converter.ConvertDecimalToMoney(productWithId.Price);
            productWithId.Stock = productWithId.Stock;

            return productWithIdGRPC;
        }

        internal static ProductWithId TansferProductAndIdToProductWithId(int id, Product product)
        {
            ProductWithId productWithId = new ProductWithId();

            productWithId.Id = id;
            productWithId.Name = product.Name;
            productWithId.Description = product.Description;
            productWithId.Price = product.Price;
            productWithId.Stock = product.Stock;

            return productWithId;
        }

        internal static PageGRPC TrasferPageToPageGRPC(Page<ProductWithId> page)
        {
            PageGRPC pageGRPC = new PageGRPC();

            pageGRPC.TotalElementsCount = page.TotalElementcCount;
            pageGRPC.TotalPagesCount = page.TotalPagesCount;
            pageGRPC.ChoosenPageNumber = page.ChoosenPageNumber;
            pageGRPC.ElementsOnPageCount = page.ElementOnPageCount;

            foreach (ProductWithId product in page.Products)
                pageGRPC.Products.Add(TransferProductWithIdToProductAndIdGRPC(product));

            return pageGRPC;
        }
    }
}
