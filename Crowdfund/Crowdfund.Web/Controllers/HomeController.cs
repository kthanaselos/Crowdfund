using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Crowdfund.Web.Models;
using Crowdfund.Core.Services.Options;
using Crowdfund.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IProjectService projectService;
        public HomeController(ILogger<HomeController> logger, IProjectService prjService)
        {
            _logger = logger;
            projectService = prjService;
        }

        public IActionResult Index()
        {
            return View(projectService.SearchProject(new SearchProjectOptions())
            .Include(p => p.Media)
            .Include(p => p.User)
            .ToList());         
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
