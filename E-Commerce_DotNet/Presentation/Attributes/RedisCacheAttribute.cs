using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction;

namespace Presentation.Attributes
{
    public class RedisCacheAttribute(int timeToLiveInSeconds = 600) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            var key = GenerateCacheKey(context.HttpContext.Request);

            var value = await cacheService.GetAsync(key);

            if(value is not null)
            {
                context.Result = new ContentResult
                {
                    Content = value,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }

            var actionExecutedContext = await next.Invoke();

            if(actionExecutedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(key, result.Value, TimeSpan.FromSeconds(timeToLiveInSeconds));
            }
        }
        private static string GenerateCacheKey(HttpRequest httpRequest)
        {
            StringBuilder key = new StringBuilder();

            key.Append(httpRequest.Path).Append("?");

            foreach (var item in httpRequest.Query.OrderBy(i => i.Key))
                key.Append($"{item.Key}={item.Value}&");

            return key.ToString().TrimEnd('&');
        }
    }
}
