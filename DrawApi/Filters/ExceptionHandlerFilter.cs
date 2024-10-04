using DrawApi.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DrawApi.Filters
{
    public class ExceptionHandlerFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null) return;
            var exceptionType = context.Exception.GetType();
            if (ExceptionMapping.ExceptionMap.TryGetValue(exceptionType, out var mapping))
            {
                context.Result = new ObjectResult(new { message = mapping.Message })
                {
                    StatusCode = mapping.StatusCode
                };
                context.ExceptionHandled = true;
            }
        }
        
    }
}
