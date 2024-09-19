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

            if (_productRepository.GetProduct(request.Id, out product))
            {
                GetProductResponse.Types.ProductFound foundedResult = new GetProductResponse.Types.ProductFound();

                foundedResult.Product = Mapper.TransferProductToProdutctInfo(product);
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

        public override async Task<GetProductsWithPaginationResponse> GetProducts(GetProductsWithPaginationRequest request, ServerCallContext context)
        {
            List<ProductInfoWithID> tempProducts = _productRepository.GetProducts();
            GetProductsWithPaginationResponse response = new GetProductsWithPaginationResponse();

            int firstElementNumberOnChoosenPage;
            int lastElementNumberOnChoosenPage;
            int currentPageNumber;
            int elementsOnCurrentPage;
            int pagesCount;

            if (request.ElementsOnPageCount < 1)
                request.ElementsOnPageCount = 1;

            pagesCount = tempProducts.Count / request.ElementsOnPageCount;

            if (tempProducts.Count % request.ElementsOnPageCount > 0 || pagesCount == 0)
                pagesCount++;

            if (request.CurrentPageNumber > pagesCount)
                currentPageNumber = pagesCount;
            else if (request.CurrentPageNumber <= 1)
                currentPageNumber = 1;
            else
                currentPageNumber = request.CurrentPageNumber;

            if (currentPageNumber < pagesCount && request.ElementsOnPageCount >= tempProducts.Count)
                elementsOnCurrentPage = request.ElementsOnPageCount;
            else if (currentPageNumber < pagesCount && request.ElementsOnPageCount <= tempProducts.Count)
                elementsOnCurrentPage = tempProducts.Count;
            else
                elementsOnCurrentPage = tempProducts.Count % request.ElementsOnPageCount;

            firstElementNumberOnChoosenPage = (currentPageNumber - 1) * request.ElementsOnPageCount + 1;

            if (tempProducts.Count - (firstElementNumberOnChoosenPage - 1) >= request.ElementsOnPageCount)
                lastElementNumberOnChoosenPage = firstElementNumberOnChoosenPage + request.ElementsOnPageCount - 1;
            else
                lastElementNumberOnChoosenPage = tempProducts.Count;

            response.AllElementsCount = tempProducts.Count;
            response.CurrentPageNumber = currentPageNumber;
            response.ElementsOnCurentPageCount = elementsOnCurrentPage;

            for (int i = firstElementNumberOnChoosenPage - 1; i < lastElementNumberOnChoosenPage; i++)
                response.Products.Add(tempProducts[i]);

            return response;
        }

        public override async Task<OperationStatusResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            Product product = Mapper.TransferProductInfoToProduct(request.Product);
            OperationStatusResponse response = new OperationStatusResponse();

            if (_productValidator.Validate(product).IsValid)
            {
                _productRepository.CreateProduct(product);
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
            Product product = Mapper.TransferProductInfoToProduct(request.Product);
            OperationStatusResponse response = new OperationStatusResponse();

            if (_productValidator.Validate(product).IsValid == false)
            {
                response.Status = Status.Failure;
                response.Message = $"Не удалось получить продукт с ID {request.Id}!";
                return response;
            }

            if (_productRepository.UpdateProduct(request.Id, product))
            {
                response.Status = Status.Success;
                response.Message = "Продукт успешно обновлен!";
            }
            else
            {
                response.Status = Status.Failure;
                response.Message = $"Не удалось получить продукт с ID {request.Id}!";
            }

            return response;
        }

        public override async Task<OperationStatusResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            if (_productRepository.DeleteProduct(request.Id))
                return new OperationStatusResponse() { Status = Status.Success, Message = "Продукт успешно удален!" };
            else
                return new OperationStatusResponse() { Status = Status.Failure, Message = $"Продукт с ID {request.Id} отсутствует в базе данных!" };
        }

        public override async Task<GetSortedProductsResponse> GetSortedProducts(GetSortedProductsRequest request, ServerCallContext context)
        {
            List<ProductInfoWithID> products = _productRepository.GetProducts();
            GetSortedProductsResponse response = new GetSortedProductsResponse();

            if (request.Argument != "Name" && request.Argument != "Price")
            {
                GetSortedProductsResponse.Types.FailureSort failureSort = new GetSortedProductsResponse.Types.FailureSort();

                failureSort.Message = $"Сортировка по параметру {request.Argument} не возможна или продукт не имет данного параметра.";
                response.FailureSort = failureSort;

                return response;
            }

            switch (request.Argument)
            {
                case "Name":
                    if (request.IsRevese == false)
                        products = products.OrderBy(product => product.Name).ToList();
                    else
                        products = products.OrderByDescending(product => product.Name).ToList();
                    break;

                case "Price":
                    if (request.IsRevese == false)
                        products = products.OrderBy(product => Converter.ConvertMoneyToDecimal(product.Price)).ToList();
                    else
                        products = products.OrderByDescending(product => Converter.ConvertMoneyToDecimal(product.Price)).ToList();
                    break;
            }

            GetSortedProductsResponse.Types.SuccessSort successSort = new GetSortedProductsResponse.Types.SuccessSort();

            foreach (var product in products)
            {
                successSort.Products.Add(product);
            }

            response.SuccessSort = successSort;

            return response;
        }

        public override async Task<GetProductsResponse> GetFiltredProducts(FilterProductsRequest request, ServerCallContext context)
        {
            List<ProductInfoWithID> products = _productRepository.GetProducts();
            GetProductsResponse response = new GetProductsResponse();

            if (request.Name != null)
                products = products.Where(product => product.Name.Contains(request.Name) == true).ToList();

            if (request.MinPrice.HasValue)
                products = products.Where(product => Converter.ConvertMoneyToDecimal(product.Price) >= request.MinPrice).ToList();

            if (request.MaxPrice.HasValue)
                products = products.Where(product => Converter.ConvertMoneyToDecimal(product.Price) <= request.MaxPrice).ToList();


            foreach (var product in products)
            {
                response.Products.Add(product);
            }

            return response;
        }
    }
}
