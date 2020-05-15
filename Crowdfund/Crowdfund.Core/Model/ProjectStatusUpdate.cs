using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Model
{
    public class ProjectStatusUpdate
    {
        public int ProjectStatusUpdateId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Status { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ProjectStatusUpdate()
        {
            Created = DateTimeOffset.Now;
        }
    }
}
