using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet("products")]
        public void GetProducts()
        {
            
        }

        [HttpGet("products/{id:int}")]
        public void GetProduct(int id)
        {

        }

        [HttpPost("products")]
        public void CreateProduct(int id, [FromQuery] string? name, [FromQuery] string? description, [FromQuery] decimal? price, [FromQuery] int? stock)
        {

        }

        [HttpPut("products/{id:int}")]
        public void UpdateProduct(int id, [FromQuery] string? name, [FromQuery] string? description, [FromQuery] decimal? price, [FromQuery] int? stock)
        {

        }

        [HttpDelete("products/{id:int}")]
        public void DeleteProduct(int id)
        {

        }
    }
}
