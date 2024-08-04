using Google.Protobuf.Collections;
using Google.Type;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using ProductService;
using ProductService.Models;
using ProductService.Repositories;
using static ProductService.ProductService;

namespace ProductService.Services
{
    public class ProductGRPCService : ProductServiceBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductValidatorService _productValidatorService;

        public ProductGRPCService(IProductRepository productRepository, ProductValidatorService productValidatorService)
        {
            _productRepository = productRepository;
            _productValidatorService = productValidatorService;          
        }

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            Product product;
            Status status;
            ProductInfo productInfo = new ProductInfo();

            if (_productRepository.GetProduct(request.Id, out product))
            {
                GetProductResponse.Types.ProductFound foundedResult = new GetProductResponse.Types.ProductFound();
                productInfo.Name = product.Name;
                productInfo.Description = product.Description;
                productInfo.Price = ConvertDecimalToMoney(product.Price);
                productInfo.Stock = product.Stock;
                foundedResult.Product = productInfo;

                return new GetProductResponse() { Status = Status.Success, Found = foundedResult };
            }
            else
            {
                GetProductResponse.Types.ProductNotFound notFoundedResult = new GetProductResponse.Types.ProductNotFound();

                notFoundedResult.Message = $"Продукт с ID {request.Id} отсутствует в базе данных"; 
                return new GetProductResponse() { Status = Status.Failure, NotFound = notFoundedResult };
            }
        }

        public override async Task<GetProductsResponse> GetProducts(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            Dictionary<int, Product> products = _productRepository.GetProducts();
            GetProductsResponse response = new GetProductsResponse();

            response.Status = Status.Success;

            foreach (var product in products)
            {
                ProductInfo productInfo = new ProductInfo();
                productInfo.Name = product.Value.Name;
                productInfo.Description = product.Value.Description;
                productInfo.Price = ConvertDecimalToMoney(product.Value.Price);
                productInfo.Stock = product.Value.Stock;
                response.Products.Add(productInfo);
            }

            return response;
        }

        public override async Task<OperationStatusResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            Product product = new Product(request.Product.Name, request.Product.Description, ConvertMoneyToDecimal(request.Product.Price), request.Product.Stock);

            if (_productValidatorService.Validate(product))
            {
                _productRepository.CreateProduct(product);
                return new OperationStatusResponse() { Status = Status.Success, Message = "Продукт успешно добавлен!" };
            }
            else
            {
                return new OperationStatusResponse() { Status = Status.Failure, Message = "Продукт не прошел валидацию!" };
            }
        }

        public override async Task<OperationStatusResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            Product product = new Product(request.Product.Name, request.Product.Description, ConvertMoneyToDecimal(request.Product.Price), request.Product.Stock);

            if (_productValidatorService.Validate(product))
            {
                if (_productRepository.UpdateProduct(request.Id, product))
                    return new OperationStatusResponse() { Status = Status.Success, Message = "Продукт успешно обновлен!" };
                else
                    return new OperationStatusResponse() { Status = Status.Failure, Message = $"Продукт с ID {request.Id} отсутствует в базе данных" };
            }
            else
            {
                return new OperationStatusResponse() { Status = Status.Failure, Message = "Продукт не прошел валидацию!" };
            }
        }

        public override async Task<OperationStatusResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            if (_productRepository.DeleteProduct(request.Id))
                return new OperationStatusResponse() { Status = Status.Success, Message = "Продукт успешно удален!" };
            else
                return new OperationStatusResponse() { Status = Status.Failure, Message = $"Продукт с ID {request.Id} отсутствует в базе данных" };
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
