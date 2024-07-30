using FluentValidation;
using ProductService.Repositories;
using ProductService.Validators;

namespace ProductService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            services.AddSingleton<ProductValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.MapGrpcService<>();
        }
    }
}
