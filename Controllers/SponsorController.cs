using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class SponsorController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public SponsorController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Voorbeeldmail()
        {
            return View();
        }

        public IActionResult Index()
        {
            string lovWebRootPath = _env.WebRootPath;
            string lovRawPathNameToFolder = @"\seed\sponsors\sponsorinfo.csv";

            SponsorListObject lovSLO = new SponsorListObject(lovWebRootPath, lovRawPathNameToFolder);

            return View(lovSLO);
        }
    }
}