using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication4.Controllers
{
    public class LinksController : Controller
    {
        private readonly ILogger<LinksController> _logger;
        private readonly IWebHostEnvironment _env;

        public LinksController(ILogger<LinksController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}