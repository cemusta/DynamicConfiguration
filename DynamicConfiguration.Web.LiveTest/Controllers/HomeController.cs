using System.Diagnostics;
using DynamicConfiguration.Web.LiveTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicConfiguration.Web.LiveTest.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IConfiguration _configuration;
        private readonly IConfigurationReader _configReader;

        public HomeController(IConfigurationReader configurationReader)
        {
            _configReader = configurationReader;
        }

        public IActionResult Index()
        {
            HomePageModel model = new HomePageModel
            {
                SupportedOs = _configReader.GetValue<string>("SupportedOs"),
                ApiVersion = _configReader.GetValue<double>("ApiVersion"),
                IsDebug = _configReader.GetValue<bool>("IsDebug"),
                Miliseconds = _configReader.GetValue<int>("Miliseconds")
            };
            
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Test pages for dynamic configuration library.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Dynamic config made by Cem Usta";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
