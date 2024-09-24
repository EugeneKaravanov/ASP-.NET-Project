using ProductService.Repositories;
using ProductService.Validators;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddSingleton<ProductValidator>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<ProductGRPCService>();
app.Run();