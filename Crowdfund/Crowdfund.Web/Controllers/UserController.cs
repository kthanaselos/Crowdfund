using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crowdfund.Web.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private IUserService userService;

        public UserController(IUserService uService)
        {
            userService = uService;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return View(userService.SearchUser(new SearchUserOptions()).ToList());
        }

        [HttpGet("{id}/details")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user = userService.SearchUser(new SearchUserOptions()
            {
                UserId = id
            }).Include(u => u.PurchasedPackages)
                    .ThenInclude(pp=>pp.Package)
                    .ThenInclude(p=>p.Project)
                .Include(u => u.Projects)
                    .ThenInclude(p=>p.Media)
                .SingleOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet("{id}/edit")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user = userService.SearchUser(new SearchUserOptions()
            {
                UserId = id
            }).SingleOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Route("search")]
        [HttpGet]
        public IActionResult Search(SearchUserOptions options)
        {
            if (options == null)
            {
                return BadRequest();
            }

            var users = userService
                .SearchUser(options)
                .ToList();

            if (users == null)
            {
                return NotFound();
            }

            return Json(users);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserOptions options)
        {
            var result = userService.CreateUser(options);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            return Json(result.Data);
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id,[FromBody] UpdateUserOptions options)
        {
            var result = userService.UpdateUser(options, id);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = userService.DeleteUser(id);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            return Ok();
        }
    }
}