using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Options;
using Crowdfund.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

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

        public IActionResult AboutUs()
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
