using Crowdfund.Core.Data;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services;
using Crowdfund.Core.Services.Options;
using System;

namespace Crowdfund
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new CrowdfundDbContext())
            {
                IUserService uService = new UserService(context);
                IProjectService pService = new ProjectService(uService, context);

                //var result = pService.CreateProject(new CreateProjectOptions()
                //{
                //    UserId = 6,
                //    Title = "fsdfsd",
                //    Description = "efds",
                //    Category = (ProjectCategory)4,
                //    FinancialGoal = 100m
                //});

                var result = pService.DeleteProject(5);
                Console.WriteLine(result.ToString());
            }
            
        }
    }
}
