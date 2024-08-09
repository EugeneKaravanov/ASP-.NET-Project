using GatewayService;
using GatewayService.Middleware;

var builder = WebApplication.CreateBuilder(args);
var address = builder.Configuration.GetValue<string>("ProductServiceAddress");

builder.Services.AddGrpcClient<Ecommerce.ProductService.ProductServiceClient>(address, options => { options.Address = new Uri(address); });
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseMiddleware<RequestLoggingMiddleware>();
app.Run();

