using Dapper;
using Npgsql;
using OrderService.Models;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _ordersConnectionString;
        private readonly string _orderItemsConnectionString;

        public OrderRepository(string ordersConnectionString, string orderItemsConnectionString)
        {
            _ordersConnectionString = ordersConnectionString;
            _orderItemsConnectionString = orderItemsConnectionString;
        }

        public async Task<Result> CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return null;
        }

        public async Task<List<Order>> GetOrdersAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return null;
        }

        public async Task<ResultWithValue<Order>> GetOrderAsync(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return null;
        }

        public async Task<List<Order>> GetOrdersByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return null;
        }
    }
}