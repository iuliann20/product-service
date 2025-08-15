//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.ComponentModel.DataAnnotations;
//using System.Net;
//using FluentValidation;

//namespace ProductService.Infrastructure.Errors
//{
//    public sealed class ProblemDetailsMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<ProblemDetailsMiddleware> _logger;

//        public ProblemDetailsMiddleware(RequestDelegate next, ILogger<ProblemDetailsMiddleware> logger)
//        {
//            _next = next; _logger = logger;
//        }

//        public async Task Invoke(HttpContext ctx)
//        {
//            try
//            {
//                await _next(ctx);
//            }
//            catch (System.ComponentModel.DataAnnotations.ValidationException vex)
//            {
//                var pd = new ValidationProblemDetails(vex.Errors.GroupBy(e => e.PropertyName)
//                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
//                {
//                    Status = StatusCodes.Status400BadRequest,
//                    Title = "Validation failed",
//                    Type = "https://httpstatuses.com/400"
//                };

//                ctx.Response.StatusCode = pd.Status.Value;
//                await ctx.Response.WriteAsJsonAsync(pd);
//            }
//            catch (UnauthorizedAccessException ua)
//            {
//                var pd = new ProblemDetails
//                {
//                    Status = StatusCodes.Status401Unauthorized,
//                    Title = "Unauthorized",
//                    Detail = ua.Message,
//                    Type = "https://httpstatuses.com/401"
//                };
//                ctx.Response.StatusCode = pd.Status.Value;
//                await ctx.Response.WriteAsJsonAsync(pd);
//            }
//            catch (InvalidOperationException ioe)
//            {
//                var pd = new ProblemDetails
//                {
//                    Status = StatusCodes.Status404NotFound,
//                    Title = "Not Found",
//                    Detail = ioe.Message,
//                    Type = "https://httpstatuses.com/404"
//                };
//                ctx.Response.StatusCode = pd.Status.Value;
//                await ctx.Response.WriteAsJsonAsync(pd);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unhandled exception");
//                var pd = new ProblemDetails
//                {
//                    Status = StatusCodes.Status500InternalServerError,
//                    Title = "Server Error",
//                    Detail = "An unexpected error occurred.",
//                    Type = "https://httpstatuses.com/500"
//                };
//                ctx.Response.StatusCode = pd.Status.Value;
//                await ctx.Response.WriteAsJsonAsync(pd);
//            }
//        }
//    }
//}
