using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Model
{
    public class Package
    {
        public int PackageId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
