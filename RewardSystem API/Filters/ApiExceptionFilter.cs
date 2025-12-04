using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RewardSystem_API.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var errorResponse = new
            {
                Message = context.Exception.Message,
                Exception = context.Exception.GetType().Name
            };

            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true; 
        }
    }
}
