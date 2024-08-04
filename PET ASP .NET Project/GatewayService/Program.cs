using GatewayService;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.Run();

