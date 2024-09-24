using GatewayService.Middleware;
using GatewayService.Filters;

var builder = WebApplication.CreateBuilder(args);
var address = builder.Configuration.GetValue<string>("ProductServiceAddress");

builder.Services.AddGrpcClient<Ecommerce.ProductService.ProductServiceClient>(address, options => { options.Address = new Uri(address); });
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomHeaderFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();
app.Run();

