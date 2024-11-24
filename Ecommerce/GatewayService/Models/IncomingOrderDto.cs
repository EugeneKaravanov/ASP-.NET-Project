namespace GatewayService.Models
{
    public class IncomingOrderDto
    {
        public int CustomerId { get; set; }
        public List<IncomingOrderItemDto> OrderItems { get; set; }
    }
}
