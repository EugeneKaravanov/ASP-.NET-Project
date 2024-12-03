using OrderService.Models;
using OrderService.Utilities;
using ProductServiceGRPC;

namespace OrderService.Services
{
    internal class Mapper
    {
        internal static TakeProductsRequest TransferListInputOrderItemToTakeProductsRequest(List<InputOrderItem> orderItems)
        {
            TakeProductsRequest takeProductsRequest = new();

            foreach (InputOrderItem item in orderItems)
            {
                InputTakeProductGRPC inputTakeProductGRPC = new();

                inputTakeProductGRPC.Id = item.ProductId;
                inputTakeProductGRPC.Quantity = item.Quantity;
                takeProductsRequest.ProductOrders.Add(inputTakeProductGRPC);
            }

            return takeProductsRequest;
        }

        internal static List<OutputOrderItem> TransferTakeProductResponseToListOutputOrderItem(TakeProductsResponse takeProductsResponse)
        {
            List<OutputOrderItem> orderItems = new();

            foreach (OutputTakeProductGRPC product in takeProductsResponse.Received.ProductOrders)
            {
                OutputOrderItem outputOrderItem = new();

                outputOrderItem.ProductId = product.Id;
                outputOrderItem.Quantity = product.Quantity;
                outputOrderItem.UnitPrice = Converter.ConvertMoneyToDecimal(product.UnitPrice);

                orderItems.Add(outputOrderItem);
            }

            return orderItems;
        }
    }
}
