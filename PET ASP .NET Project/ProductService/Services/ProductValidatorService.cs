using ProductService.Models;
using ProductService.Validators;

namespace ProductService.Services
{
    public class ProductValidatorService
    {
        private readonly ProductValidator _productValidator;

        public ProductValidatorService(ProductValidator productValidator) 
        {
            _productValidator = productValidator;
        }

        public bool Validate(Product product)
        {
            return _productValidator.Validate(product).IsValid;
        }
    }
}
