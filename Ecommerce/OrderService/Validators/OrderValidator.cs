using FluentValidation;
using OrderService.Models;

namespace OrderService.Validators
{
    public class OrderValidator : AbstractValidator<>
    {
        private readonly int maxNameLength = 100;
        private readonly int minPrice = 0;
        private readonly int minStock = 0;

        public OrderValidator() 
        {
            RuleFor(product => product.Name).NotEmpty().MaximumLength(maxNameLength);
            RuleFor(product => product.Price).GreaterThan(minPrice);
            RuleFor(product => product.Stock).GreaterThanOrEqualTo(minStock);
        }
    }
}
