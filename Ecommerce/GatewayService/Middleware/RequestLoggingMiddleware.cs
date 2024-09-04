using Microsoft.AspNetCore.Http.Extensions;

namespace GatewayService.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public RequestLoggingMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Logfile запроса:");
            Console.WriteLine($"Метод - {context.Request.Method}");
            Console.WriteLine($"URL - {context.Request.GetDisplayUrl}");

            await _requestDelegate(context);

            Console.WriteLine($"Статус ответа - {context.Response.StatusCode}");
            Console.WriteLine($"Время ответа - {DateTime.Now}");
        }
    }
}
