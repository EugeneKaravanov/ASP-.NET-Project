using Ecommerce;
using ProductService.Models;
using ProductService.Repositories;
using System.Collections.Concurrent;

namespace TestProject
{
    public class InMemoryProductRepositoryTests
    {
        [Fact]
        public void TestConcurentProductCreation()
        {
            InMemoryProductRepository productRepository = new();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 100000; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    Product product = new Product();
                    product.Name = "Продукт " + i.ToString();
                    productRepository.CreateProduct(product);
                }));
            }

            Task.WaitAll(tasks.ToArray());

            GetProductsRequest request = new GetProductsRequest();
            request.ElementsOnPageCount = 100000;
            request.ChoosenPageNumber = 1;

            Page<ProductWithId> page = productRepository.GetProducts(request);
            Assert.Equal(100000, page.TotalElementcCount);
        }
    }
}