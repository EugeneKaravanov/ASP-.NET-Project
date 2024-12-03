using Grpc.Core;
using OrderServiceGRPC;
using static OrderServiceGRPC.OrderServiceGRPC;

namespace OrderService.Services
{
    public class OrderGRPCService : OrderServiceGRPCBase
    {
        public override Task<OperationStatusResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            return base.CreateOrder(request, context);
        }

        public override 
    }
}
