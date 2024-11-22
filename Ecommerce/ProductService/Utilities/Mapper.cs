using Ecommerce;
using ProductService.Models;

namespace ProductService.Utilities
{
    internal class Mapper
    {
        internal static ProductGRPC TransferProductAndIdToProductGRPC(int id, Product product)
        {
            ProductGRPC productGrpc = new ProductGRPC();

            productGrpc.Id = id;
            productGrpc.Name = product.Name;
            productGrpc.Description = product.Description;
            productGrpc.Price = Converter.ConvertDecimalToMoney(product.Price);
            productGrpc.Stock = product.Stock;

            return productGrpc;
        }

        internal static Product TransferProductGRPCToProductAndId(ProductGRPC productGrpc, out int id)
        {
            Product product = new Product();

            id = productGrpc.Id;
            product.Name = productGrpc.Name;
            product.Description = productGrpc.Description;
            product.Price = Converter.ConvertMoneyToDecimal(productGrpc.Price);
            product.Stock = productGrpc.Stock;

            return product;
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

        internal static ProductGRPC TransferProductWithIdToProductGrpc(ProductWithId productWithId)
        {
            ProductGRPC productGrpc = new ProductGRPC();

            productGrpc.Id = productWithId.Id;
            productGrpc.Name = productWithId.Name;
            productGrpc.Description = productWithId.Description;
            productGrpc.Price = Converter.ConvertDecimalToMoney(productWithId.Price);
            productGrpc.Stock = productWithId.Stock;

            return productGrpc;
        }

        internal static PageGRPC TrasferPageToPageGRPC(Page<ProductWithId> page)
        {
            PageGRPC pageGRPC = new PageGRPC();

            pageGRPC.TotalElementsCount = page.TotalElementcCount;
            pageGRPC.TotalPagesCount = page.TotalPagesCount;
            pageGRPC.ChoosenPageNumber = page.ChoosenPageNumber;
            pageGRPC.ElementsOnPageCount = page.ElementOnPageCount;

            foreach (ProductWithId product in page.Products)
                pageGRPC.Products.Add(TransferProductWithIdToProductGrpc(product));

            return pageGRPC;
        }

        internal static Ecommerce.Status TransferResultStatusToResponseStatus(Models.Status status)
        {
            switch (status)
            {
                case Models.Status.Success:
                    return Ecommerce.Status.Success;

                case Models.Status.NotFound:
                    return Ecommerce.Status.NotFound;

                default:
                    return Ecommerce.Status.Failure;
            }
        }
    }
}
