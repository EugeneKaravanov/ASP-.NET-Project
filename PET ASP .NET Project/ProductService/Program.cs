using ProductService;

await WebApplication
    .CreateBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build()
    .StartAsync();
