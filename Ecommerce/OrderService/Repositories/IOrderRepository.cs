using OrderService.Models;

namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        public Task<Result> CreateOrder(Order order, CancellationToken cancellationToken = default);
        public Task<List<Order>> GetOrdersAsync(CancellationToken cancellationToken = default);
        public Task<Order> GetOrderAsync(int id, CancellationToken cancellationToken = default);
        public Task<List<Order>> GetOrdersByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    }
}
