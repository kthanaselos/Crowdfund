using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Model
{
    public class ProjectMedia
    {
        public int ProjectId { get; set; }
        public string MediaUrl { get; set; }
        public Project Project { get; set; }
    }
}
