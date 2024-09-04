using ProductService.Models;

namespace GatewayService.Models
{
    public class ProductWithIdDto
    {
        public int Id { get; set; }
        public ProductDto Product { get; set; }
    }
}
