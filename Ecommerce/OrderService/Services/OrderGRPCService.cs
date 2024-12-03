using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OrderServiceGRPC;
using OrderService.Repositories;
using static OrderServiceGRPC.OrderServiceGRPC;

namespace OrderService.Services
{
    public class OrderGRPCService : OrderServiceGRPCBase
    {
        private readonly IOrderRepository _repository;

        public OrderGRPCService(IOrderRepository repository)
        {
            IOrderRepository _repository = repository;
        }

        public override Task<OperationStatusResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            return base.CreateOrder(request, context);
        }

        public override Task<GerOrdersResponse> GetOrders(Empty request, ServerCallContext context)
        {
            return base.GetOrders(request, context);
        }

        public override Task<GetOrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            return base.GetOrder(request, context);
        }

        public override Task<GerOrdersResponse> GerOrdersByCustomer(GerOrderByCustomerRequest request, ServerCallContext context)
        {
            return base.GerOrdersByCustomer(request, context);
        }
    }
}
