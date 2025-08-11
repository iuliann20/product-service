using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Helpers
{
    public class DeprecatedRouteFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<ObsoleteAttribute>().Any())
            {
                context.Result = new ObjectResult(new
                {
                    Status = 410,
                    Message = "This endpoint is deprecated. Please check the new list of endpoints."
                })
                {
                    StatusCode = 410
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
