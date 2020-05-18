using Crowdfund.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Services.Options
{
    public class CreatePackageOptions
    {
        public int ProjectId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
