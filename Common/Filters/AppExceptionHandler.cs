using Ecommerce.Common.Exceptions;
using Ecommerce.Common.Models.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce.Common.Filters
{
    public class AppExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                UnauthorizedException ex => new UnauthorizedObjectResult(new ErrorResponse
                {
                    Success = false,
                    Message = "Unauthorized",
                    Errors = new string[] { ex.Message }
                }),
                BadRequestException ex => new BadRequestObjectResult(new ErrorResponse
                {
                    Success = false,
                    Message = "Bad Request",
                    Errors = ex.Errors
                }),
                ValidationException ex => new BadRequestObjectResult(new ErrorResponse
                {
                    Success = false,
                    Message = "Bad Request",
                    Errors = ex.Errors.Select(x => x.ErrorMessage)
                }),
                NotFoundException => new NotFoundObjectResult(new ErrorResponse
                {
                    Success = false,
                    Message = "Not Found"
                }),
                _ => new ObjectResult(new ErrorResponse
                {
                    Success = false,
                    Message = "Internal Server Error"
                })
                { StatusCode = 500 },
            };
        }
    }
}
