using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce.Common.Filters
{
    public class AppExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new Response
            {
                Success = false,
                Message = context.Exception.Message
            };
            if (context.Exception is UnauthorizedException)
            {
                context.Result = new UnauthorizedObjectResult(response);
            }
            else if (context.Exception is BadRequestException)
            {
                context.Result = new BadRequestObjectResult(response);
            }
            else if (context.Exception is NotFoundException)
            {
                context.Result = new NotFoundObjectResult(response);
            }
            else
            {
                response.Message = "Internal Server Error";
                context.Result = new ObjectResult(response) { StatusCode = 500 };
            }
        }
    }
}
