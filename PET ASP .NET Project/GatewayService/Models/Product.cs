using System.Diagnostics.Metrics;

namespace ProductService.Models
{
    public class Product
    {
        public string Name;
        public string Description;
        public decimal Price;
        public int Stock;

        public Product(string name, string description, decimal price, int stock)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }
    }
}
