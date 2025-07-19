using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.MVC.Filters
{
    public class LogActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Controller Action çalıştırılmadan önce bu metot çalışır.

            string cnrllName = context.RouteData.Values["controller"].ToString();
            string actionName = context.RouteData.Values["action"].ToString();

            Console.WriteLine($"Executing: {cnrllName}.{actionName} at {DateTime.Now}");

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Controller Action çalıştırılmadan sonra bu metot çalışır.
            string cnrllName = context.RouteData.Values["controller"].ToString();
            string actionName = context.RouteData.Values["action"].ToString();

            Console.WriteLine($"Executed: {cnrllName}.{actionName} at {DateTime.Now}"); ;
        }
    }
}
