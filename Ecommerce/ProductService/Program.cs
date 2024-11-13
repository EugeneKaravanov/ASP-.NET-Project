using ProductService.Repositories;
using ProductService.Validators;
using ProductService.Services;
using System.Data;
using Npgsql;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner;
using System.Reflection;
using ProductService.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddSingleton<ProductValidator>();
builder.Services.AddScoped<IDbConnection>(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new NpgsqlConnection(connectionString);
});

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
        .ScanIn(typeof(InitialMigration).Assembly).For.Migrations());
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<ProductGRPCService>();
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.Run();