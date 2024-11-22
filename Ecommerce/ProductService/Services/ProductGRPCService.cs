﻿using Ecommerce;
using Grpc.Core;
using ProductService.Models;
using ProductService.Repositories;
using ProductService.Utilities;
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

        public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
        {
            PageGRPC pageGRPC = Mapper.TrasferPageToPageGRPC(await _productRepository.GetProductsAsync(request, context.CancellationToken));
            GetProductsResponse getProductsResponse = new GetProductsResponse();

            getProductsResponse.Page = pageGRPC;

            return getProductsResponse;
        }

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            GetProductResponse response = new GetProductResponse();
            ResultWithValue<ProductWithId> result = await _productRepository.GetProduct(request.Id, context.CancellationToken);

            if (result.Status == Models.Status.Success)
            {
                GetProductResponse.Types.ProductFound foundedResult = new GetProductResponse.Types.ProductFound();

                foundedResult.Product = Mapper.TransferProductWithIdToProductGrpc(result.Value);
                response.Found = foundedResult;

                return response;
            }
            else
            {
                GetProductResponse.Types.ProductNotFound notFoundedResult = new GetProductResponse.Types.ProductNotFound();

                notFoundedResult.Message = result.Message;
                response.NotFound = notFoundedResult;

                return response;
            }
        }

        public override async Task<OperationStatusResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            Product product = Mapper.TransferProductGRPCToProductAndId(request.Product, out int id);
            OperationStatusResponse response = new OperationStatusResponse();

            if (_productValidator.Validate(product).IsValid)
            {
                Result result = await _productRepository.CreateProduct(product, context.CancellationToken);
                
                response.Status = Mapper.TransferResultStatusToResponseStatus(result.Status);
                response.Message = result.Message;

                return response;
            }
            else
            {
                response.Status = Status.Failure;
                response.Message = "Продукт не прошел валидацию!";

                return response;
            }
        }

        public override async Task<OperationStatusResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            int id;
            Result result;
            Product product = Mapper.TransferProductGRPCToProductAndId(request.Product, out id);
            OperationStatusResponse response = new OperationStatusResponse();

            if (_productValidator.Validate(product).IsValid == false)
            {
                response.Status = Status.Failure;
                response.Message = $"Не удалось обновить продукт с ID {id}, так как изменения не прошли валидацию!";

                return response;
            }

            result = await _productRepository.UpdateProduct(id, product, context.CancellationToken);
            response.Status = Mapper.TransferResultStatusToResponseStatus(result.Status);
            response.Message = result.Message;

            return response;
        }

        public override async Task<OperationStatusResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            OperationStatusResponse response = new OperationStatusResponse();
            Result result = await _productRepository.DeleteProduct(request.Id, context.CancellationToken);

            response.Status = Mapper.TransferResultStatusToResponseStatus(result.Status);
            response.Message = result.Message;

            return response;
        }
    }
}
