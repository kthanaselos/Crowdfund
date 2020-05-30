using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Options;
using Microsoft.AspNetCore.Mvc;

namespace Crowdfund.Web.Controllers
{
    [Route("[controller]")]
    public class PackageController : Controller
    {
        IPackageService packageService;
        public PackageController(IPackageService pckgService)
        {
            packageService = pckgService;
        }

        [HttpPost("[action]")]
        public IActionResult Create([FromBody] CreatePackageOptions options)
        {
            var result = packageService.CreatePackage(options);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            var result = packageService.DeletePackage(id);

            if (!result.Success)
            {
                return StatusCode((int)result.ErrorCode, result.ErrorText);
            }

            return Ok();
        }
    }
}