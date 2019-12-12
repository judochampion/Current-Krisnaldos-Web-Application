using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class SponsorController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public SponsorController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Voorbeeldmail()
        {
            return View();
        }
    }
}