using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Services.Options
{
    public class SearchPackageOptions
    {
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int? PackageId { get; set; }
    }
}
