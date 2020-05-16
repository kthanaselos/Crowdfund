using Crowdfund.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Services.Options
{
    public class UpdateProjectOptions
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectCategory? Category { get; set; }        
    }
}
