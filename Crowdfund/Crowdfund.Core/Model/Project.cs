using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Model
{
    public class Project
    {
        public int ProjectId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }
        public decimal FinancialGoal { get; set; }
        public decimal FinancialProgress { get; set; }
        public List<ProjectMedia> Media { get; set; }
        public List<ProjectStatusUpdate> StatusUpdates { get; set; }
        public List<Package> Packages { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public Project()
        {
            Created = DateTimeOffset.Now;
            Media = new List<ProjectMedia>();
            StatusUpdates = new List<ProjectStatusUpdate>();
            Packages = new List<Package>();
            FinancialProgress = 0M;
        }
    }
}
