using Microsoft.AspNetCore.Builder;
using ProductService.Repositories;
using ProductService.Services;
using ProductService.Validators;

namespace ProductService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            services.AddSingleton<ProductValidator>();
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapGrpcService<ProductServiceGRPC>();
            });
        }
    } 
}
