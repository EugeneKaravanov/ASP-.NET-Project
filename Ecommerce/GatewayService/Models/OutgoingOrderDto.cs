namespace GatewayService.Models
{
    public class OutgoingOrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OutgoingOrderItemDto> OrderItems { get; set; }
    }
}
