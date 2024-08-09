using Microsoft.AspNetCore.Mvc.Filters;

namespace GatewayService.Filters
{
    public class CustomHeaderFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("X-Developer-Name", "YourName");
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}
