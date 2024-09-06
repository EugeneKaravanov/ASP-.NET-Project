using Ecommerce;
using Google.Protobuf.Collections;
using Google.Type;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using ProductService;
using ProductService.Models;
using ProductService.Repositories;
using ProductService.Validators;
using static Ecommerce.ProductService;
using Status = Ecommerce.Status;

namespace ProductService.Services
{
    public class ProductGRPCService : ProductServiceBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ProductValidator _productValidator;

        public ProductGRPCService(IProductRepository productRepository, ProductValidator productValidator)
        {
            _productRepository = productRepository;
            _productValidator = productValidator;          
        }

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            Product product;
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
                ProductInfoWithID productInfoWithID = new ProductInfoWithID();

                productInfoWithID.Id = product.Key;
                productInfoWithID.Product = new ProductInfo();
                productInfoWithID.Product.Name = product.Value.Name;
                productInfoWithID.Product.Description = product.Value.Description;
                productInfoWithID.Product.Price = ConvertDecimalToMoney(product.Value.Price);
                productInfoWithID.Product.Stock = product.Value.Stock;
                response.Products.Add(productInfoWithID);
            }

            return response;
        }

        public override async Task<OperationStatusResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            Product product = new Product();

            product.Name = request.Product.Name;
            product.Description = request.Product.Description;
            product.Price = ConvertMoneyToDecimal(request.Product.Price);
            product.Stock = request.Product.Stock;

            if (_productValidator.Validate(product).IsValid)
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
            Product product = new Product();

            product.Name = request.Product.Name;
            product.Description = request.Product.Description;
            product.Price = ConvertMoneyToDecimal(request.Product.Price);
            product.Stock = request.Product.Stock;

            if (_productValidator.Validate(product).IsValid)
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
