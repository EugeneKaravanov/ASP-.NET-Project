namespace GatewayService.Models
{
    public class FilterRequestDto
    {
        public string? Name { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
