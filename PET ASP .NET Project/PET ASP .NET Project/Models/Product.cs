using System.Diagnostics.Metrics;

namespace PET_ASP_.NET_Project.Models
{
    public class Product
    {
        private static int Counter = 0;

        public int Id;
        public string Name;
        public string Description;
        public decimal Price;
        public int Stock;

        public Product(string name, string description, decimal price, int stock)
        {
            Id = Counter++;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }
    }
}
