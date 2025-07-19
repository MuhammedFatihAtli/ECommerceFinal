using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ECommerce.MVC.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception occurred.");

            // API ise JSON, değilse Error view döndür
            if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
            {
                context.Result = new ObjectResult(new { error = context.Exception.Message })
                {
                    StatusCode = 500
                };
            }
            else
            {
                context.Result = new ViewResult
                {
                    ViewName = "/Views/Shared/Error.cshtml",
                    ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), context.ModelState)
                    {
                        { "ErrorMessage", context.Exception.Message }
                    }
                };
            }
            context.ExceptionHandled = true;
        }
    }
} 