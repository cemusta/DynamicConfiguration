using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DynamicConfiguration.Data.Model;
using DynamicConfiguration.Data.Repository;
using DynamicConfiguration.Web.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace DynamicConfiguration.Web.Admin.Controllers
{
    public class HomeController : Controller
    {
        private IConfigurationRepository _repo;

        public HomeController(IConfigurationRepository repository)
        {
            _repo = repository;
        }

        public IActionResult Index()
        {
            List<Configuration> allConfig = _repo.Repository.All().ToList();
            return View(allConfig);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Admin gui for dynamic configuration";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
