using GatewayService;

var builder = WebApplication.CreateBuilder(args);
var address = builder.Configuration.GetValue<string>("ProductServiceAddress");
builder.Services.AddGrpcClient<Ecommerce.ProductService.ProductServiceClient>(address, options => { options.Address = new Uri(address); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.Run();

