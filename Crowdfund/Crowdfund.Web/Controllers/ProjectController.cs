using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.OleDb;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Options;
using Crowdfund.Core.Services.Options.CreateOptions;
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

        [HttpGet("{id}/[action]")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var project = projectService.SearchProject(new SearchProjectOptions()
            {
                ProjectId = id
            }).Include(p => p.Media)
                .Include(p => p.StatusUpdates)
                .Include(p => p.Packages)
                .SingleOrDefault();

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpGet("[action]")]
        public IActionResult Create()
        {
            var dummyProject = new Project();
            return View(dummyProject);
        }

        [HttpPost("[action]")]
        public IActionResult Create([FromBody] CreateProjectOptions options)
        {
            var result = projectService.CreateProject(options);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            //return Ok();
            return Json(new {
                projectId = result.Data.ProjectId,
                description = result.Data.Description,
                title = result.Data.Title
            });
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateProjectOptions options)
        {
            var result = projectService.UpdateProject(options, id);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult PostStatus([FromBody] CreateProjectStatusOptions options)
        {
            return View();
        }

        [HttpPost("{id}/[action]")]
        public IActionResult PostStatus(int id, [FromBody] CreateProjectStatusOptions options)
        {
            var result = projectService.CreateStatusUpdate(options, id);
            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            return Ok();
        }

        [Route("search2")]
        [HttpGet]
        public IActionResult Search2()
        {
            return View();
        }

        [Route("search")]
        [HttpGet]
        public IActionResult Search(SearchProjectOptions options)
        {
            if (options == null)
            {
                return BadRequest();
            }

            var projects = projectService
                .SearchProject(options)
                .Include(p => p.Media)
                .Include(p => p.StatusUpdates)
                .Include(p => p.Packages)
                .Include(p => p.User)
                .ToList();

            if (projects == null)
            {
                return NotFound();
            }

            //return Json(projects);
            return View("Index", projects);
        }

        [HttpDelete("{id}/[action]")]
        public IActionResult Delete(int id)
        {
            var result = projectService.DeleteProject(id);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            return Ok();
        }
    }
}