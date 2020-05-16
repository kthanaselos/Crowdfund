using Crowdfund.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Services.Options
{
    public class SearchProjectOptions
    {
        public string Title { get; set; }
        public int? ProjectId { get; set; }
        public ProjectCategory? Category { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }

    }
}
