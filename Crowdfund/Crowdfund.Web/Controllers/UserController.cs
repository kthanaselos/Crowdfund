using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Options;
using Microsoft.AspNetCore.Mvc;

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
            }).SingleOrDefault();

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

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateUserOptions options)
        {
            if (userService.UpdateUser(options, id))
            {
                return Ok();
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [Route("search")]
        [HttpGet]
        public IActionResult Search(SearchUserOptions options)
        {
            if (options == null)
            {
                return BadRequest();
            }

            var customers = userService
                .SearchUser(options)
                .ToList();

            if (customers == null)
            {
                return NotFound();
            }

            return Json(customers);
        }
    }
}