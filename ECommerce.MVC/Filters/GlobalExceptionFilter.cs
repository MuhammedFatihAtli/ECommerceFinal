using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ECommerce.MVC.Filters
{
    // Uygulama genelinde istisnaları yakalamak için kullanılan filtre sınıfı
    public class GlobalExceptionFilter : IExceptionFilter
    {
        // Logger nesnesi: Hataları loglamak için kullanılır

        private readonly ILogger<GlobalExceptionFilter> _logger;
        // Yapıcı metod: Logger bağımlılığı enjekte edilir
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        // İstisna yakalandığında çalışacak metod (IExceptionFilter arayüzünden gelir)
        public void OnException(ExceptionContext context)
        { // Yakalanan istisna loglanır
            _logger.LogError(context.Exception, "Unhandled exception occurred.");
            // Eğer gelen istek /api ile başlıyorsa, JSON formatında hata döndür
            // API ise JSON, değilse Error view döndür
            if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
            {
                context.Result = new ObjectResult(new { error = context.Exception.Message })
                { // JSON formatında hata mesajı ve 500 HTTP durumu döndürülür
                    StatusCode = 500
                };
            }
            else
            { // API dışındaki isteklerde, Error.cshtml view’ı döndürülür
                context.Result = new ViewResult
                {
                    ViewName = "/Views/Shared/Error.cshtml",
                    // ViewData içine hata mesajı eklenir
                    ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), context.ModelState)
                    {
                        { "ErrorMessage", context.Exception.Message }
                    }
                };
            }
            // Hatanın uygulama tarafından ele alındığı belirtilir, böylece başka bir yere iletilmez
            context.ExceptionHandled = true;
        }
    }
} 