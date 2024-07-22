using FluentValidation;
using PET_ASP_.NET_Project.Validators;

namespace PET_ASP_.NET_Project
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<ProductValidator>();
        }

        public void Configure(IApplicationBuilder app)
        {

        }
    }
}
