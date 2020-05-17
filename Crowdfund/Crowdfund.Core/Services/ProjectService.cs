using Crowdfund.Core.Data;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Crowdfund.Core.Services
{
    public class ProjectService : IProjectService
    {
        private CrowdfundDbContext dbContext;
        private IUserService userService;
        //private IPackageService packageService;

        public ProjectService(IUserService uService, CrowdfundDbContext context)
        {
            dbContext = context;
            userService = uService;
            //packageService = new PackageService(dbContext);
        }

        public Project CreateProject(CreateProjectOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var user = userService.SearchUser(new SearchUserOptions()
            {
                UserId = options.UserId
            }).Include(u => u.Projects)
            .ThenInclude(p => p.Packages)
            .SingleOrDefault();

            if (user == null)
            {
                return null;
            }

            if (IsValidProjectOptions(options))
            {
                Project project = new Project();

                project.Title = options.Title;

                project.Description = options.Description;

                project.Category = options.Category;

                project.FinancialGoal = options.FinancialGoal;

                user.Projects.Add(project);

                if (dbContext.SaveChanges() > 0)
                {
                    return project;
                }
            }

            return null;
        }

        public bool IsValidProjectOptions(CreateProjectOptions options)
        {
            if ((string.IsNullOrWhiteSpace(options.Title)||
                string.IsNullOrWhiteSpace(options.Description)||
                !Enum.IsDefined(typeof(ProjectCategory),options.Category)) ||
                options.FinancialGoal<=0m)
            {
                return false;
            }

            return true;
        }

        public bool DeleteProject(int id)
        {
            var project = SearchProject(new SearchProjectOptions()
            {
                ProjectId = id
            }).Include(p => p.Packages).SingleOrDefault();

            dbContext.Remove(project);
            if (dbContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;

        }

        public IQueryable<Project> SearchProject(SearchProjectOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var query = dbContext
                .Set<Project>()
                .AsQueryable();

            if (options.ProjectId != null)
            {
                query = query.Where(p => p.ProjectId == options.ProjectId);
            }

            if (options.Title != null)
            {
                query = query.Where(p => p.Title == options.Title);
            }

            if (options.Description != null)
            {
                query = query.Where(p => p.Description == options.Description);
            }

            if (options.Category != null)
            {
                query = query.Where(p => p.Category == options.Category.Value);
            }

            if (options.UserId != null)
            {
                query = query.Where(p => p.UserId == options.UserId.Value);
            }

            return query;
        }

        public Project UpdateProject(UpdateProjectOptions options,int id)
        {
            if (options == null || id == 0)
            {
                return null;
            }

            var project = dbContext
                .Set<Project>()
                .Where(p => p.ProjectId == id)
                .Include(p => p.Packages)
                .SingleOrDefault();

            if (project == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(options.Title))
            {
                project.Title = options.Title;
            }

            if (!string.IsNullOrWhiteSpace(options.Description))
            {
                project.Description = options.Description;
            }

            if (options.Category != null)
            {
                project.Category = options.Category.Value;
            }

            if (dbContext.SaveChanges() > 0)
            {
                return project;
            }

            return null;
        }
    }
}
