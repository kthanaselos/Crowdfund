using Crowdfund.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Services.Options
{
    public class CreateProjectOptions
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ProjectCategory Category { get; set; }
        public decimal FinancialGoal { get; set; }
    }
}
