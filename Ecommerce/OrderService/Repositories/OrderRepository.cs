using Dapper;
using Npgsql;
using OrderService.Models;

namespace OrderService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _сonnectionString;

        public OrderRepository(string сonnectionString, string orderItemsConnectionString)
        {
            _сonnectionString = сonnectionString;
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