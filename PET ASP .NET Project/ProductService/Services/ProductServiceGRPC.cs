using Google.Type;
using Grpc.Core;
using ProductService;
using ProductService.Models;
using ProductService.Repositories;
using static ProductService.ProductService;

namespace ProductService.Services
{
    public class ProductServiceGRPC : ProductServiceBase
    {
        private readonly IProductRepository _productRepository;

        public ProductServiceGRPC(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public override async Task<GetProductRespone> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            Product product;
            OperationStatusResponse operationStatusResponse = new OperationStatusResponse();
            ProductInfo productInfo = new ProductInfo();
            GetProductRespone response;

            if (_productRepository.GetProduct(request.Id, out product))
            {
                
                productInfo.Name = product.Name;
                productInfo.Description = product.Description;
                productInfo.Price = ConvertDecimalToMoney(product.Price);
                productInfo.Stock = product.Stock;
                operationStatusResponse.Status = true;

                return response = new GetProductRespone() { Status = operationStatusResponse, Product = productInfo };
            };

            operationStatusResponse.Status = false;
            return response = new GetProductRespone() { Status = operationStatusResponse, Product = productInfo };
        }

        private static Money ConvertDecimalToMoney(decimal value)
        {
            Money money = new Money();

            money.Units = (long)value;
            money.Nanos = (int)(value % 1);
            money.CurrencyCode = "RUB";

            return money;
        }

        private static decimal ConvertMoneyToDecimal(Money money)
        {
            decimal price = money.Units + money.Nanos;

            return price;
        }
    }
}
