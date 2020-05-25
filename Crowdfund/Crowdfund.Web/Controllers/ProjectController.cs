using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund.Web.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        IProjectService projectService;
        public ProjectController(IProjectService prjService)
        {
            projectService = prjService;
        }

        [HttpGet("[action]")]
        public IActionResult Index()
        {
            return View(projectService.SearchProject(new SearchProjectOptions())
                .Include(p=>p.Media)
                .Include(p=>p.User)
                .ToList());
        }

        [HttpGet("{id}/[action]")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var project = projectService.SearchProject(new SearchProjectOptions()
            {
                ProjectId = id
            }).Include(p => p.Media)
                .Include(p=>p.StatusUpdates)
                .Include(p=>p.Packages)
                .Include(p => p.User)
                .SingleOrDefault();

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }
    }
}