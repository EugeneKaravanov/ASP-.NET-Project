using Ecommerce;
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

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            Product product;
            GetProductResponse response = new GetProductResponse();

            if (_productRepository.GetProduct(request.Id, out product, context.CancellationToken))
            {
                GetProductResponse.Types.ProductFound foundedResult = new GetProductResponse.Types.ProductFound();

                foundedResult.Product = Mapper.TransferProductToProductGrpc(product);
                response.Found = foundedResult;

                return response;
            }
            else
            {
                GetProductResponse.Types.ProductNotFound notFoundedResult = new GetProductResponse.Types.ProductNotFound();

                notFoundedResult.Message = $"Продукт с ID {request.Id} отсутствует в базе данных";
                response.NotFound = notFoundedResult;

                return response;
            }
        }

        public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
        {
            PageGRPC pageGRPC = Mapper.TrasferPageToPageGRPC(_productRepository.GetProducts(request, context.CancellationToken));
            GetProductsResponse getProductsResponse = new GetProductsResponse();

            getProductsResponse.Page = pageGRPC;

            return getProductsResponse;
        }

        public override async Task<OperationStatusResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            Product product = Mapper.TransferProductGRPCToProduct(request.Product);
            OperationStatusResponse response = new OperationStatusResponse();

            if (_productValidator.Validate(product).IsValid)
            {
                _productRepository.CreateProduct(product, context.CancellationToken);
                response.Status = Status.Success;
                response.Message = "Продукт успешно добавлен!";

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
            Product product = Mapper.TransferProductWithIdGRPCToProductAndId(request.Product, out id);
            OperationStatusResponse response = new OperationStatusResponse();

            if (_productValidator.Validate(product).IsValid == false)
            {
                response.Status = Status.Failure;
                response.Message = $"Не удалось получить продукт с ID {id}!";
                return response;
            }

            if (_productRepository.UpdateProduct(id, product, context.CancellationToken))
            {
                response.Status = Status.Success;
                response.Message = "Продукт успешно обновлен!";
            }
            else
            {
                response.Status = Status.Failure;
                response.Message = $"Не удалось получить продукт с ID {id}!";
            }

            return response;
        }

        public override async Task<OperationStatusResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            OperationStatusResponse response = new OperationStatusResponse();

            if (_productRepository.DeleteProduct(request.Id, context.CancellationToken))
            {
                response.Status = Status.Success;
                response.Message = "Продукт успешно удален!";

                return response;
            }
            else
            {
                response.Status = Status.Failure;
                response.Message = $"Продукт с ID {request.Id} отсутствует в базе данных!";

                return response;
            }
        }
    }
}
