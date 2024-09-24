using FluentValidation;
using ProductService.Models;

namespace ProductService.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        private readonly int maxNameLength = 100;
        private readonly int minPrice = 0;
        private readonly int minStock = 0;

        public ProductValidator() 
        {
            RuleFor(product => product.Name).NotEmpty().MaximumLength(maxNameLength);
            //RuleFor(product => product.Price).GreaterThan(minPrice);
            RuleFor(product => product.Stock).GreaterThanOrEqualTo(minStock);
        }
    }
}
