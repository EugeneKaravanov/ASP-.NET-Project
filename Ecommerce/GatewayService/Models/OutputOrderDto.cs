namespace GatewayService.Models
{
    public class OutputOrderDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OutgoingOrderItemDto> OrderItems { get; set; }
    }
}
