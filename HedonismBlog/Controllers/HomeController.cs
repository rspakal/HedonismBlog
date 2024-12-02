using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HedonismBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Returning view: Home/Index");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        //[Route("/Error")]
        public IActionResult Error(int? statusCode = null)
        {
            ViewBag.StatusCode = statusCode.ToString();

            if (statusCode == 404)
            {
                return View("Error404");
            }
            else
            {
                return View("Error500");
            }
        }
        public IActionResult Error500()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null)
            {
                var exception = exceptionHandlerPathFeature.Error;
                var path = exceptionHandlerPathFeature.Path;
                _logger.LogError($"An error occurred: '{exception.Message}' in {path}");
            }
            return View();
        }
    }
}
