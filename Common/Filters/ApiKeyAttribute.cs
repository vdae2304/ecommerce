using Ecommerce.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce.Common.Filters
{
    public class ApiKeyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var config = context.HttpContext.RequestServices.GetService<IConfiguration>()
                ?? throw new NullReferenceException();
            string? apiKey = context.HttpContext.Request.Headers["x-api-key"];
            if (apiKey != config["Authentication:ApiKey"])
            {
                context.Result = new UnauthorizedObjectResult(new Response
                {
                    Success = false,
                    Message = "Invalid api-key"
                });
            }
        }
    }
}
