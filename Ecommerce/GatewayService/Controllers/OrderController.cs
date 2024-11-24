using FluentMigrator.Builder.Create.Index;
using GatewayService.Models;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Controllers
{
    public class OrderController : Controller
    {
        [HttpPost("orders")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] IncomingOrderDto incomingOrderDto, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpGet("orders")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<List<OutgoingOrderDto>> GetOrders(CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpGet("orders/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(int id, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpPut("orders/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] IncomingOrderDto incomingOrderDto, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpDelete("orders/{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
        {
            return null;
        }

        [HttpDelete("orders/by-customer/{customerId}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<List<OutgoingOrderDto>> GetOrdersByCustomer(int customerId, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
