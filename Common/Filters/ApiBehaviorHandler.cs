using Ecommerce.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Common.Filters
{
    public class ApiBehaviorHandler
    {
        public static IActionResult InvalidModelState(ActionContext context)
        {
            var errors = context.ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage);
            return new BadRequestObjectResult(new Response
            {
                Success = false,
                Message = string.Join(" ", errors)
            });
        }
    }
}
