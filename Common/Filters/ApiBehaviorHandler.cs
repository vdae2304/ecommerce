using Ecommerce.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Common.Filters
{
    public class ApiBehaviorHandler
    {
        public static IActionResult InvalidModelState(ActionContext context)
        {
            return new BadRequestObjectResult(new ErrorResponse
            {
                Success = false,
                Message = "Bad Request",
                Errors = context.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
            });
        }
    }
}
