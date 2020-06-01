using Crowdfund.Core.Data;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using Crowdfund.Core.Services.Options.CreateOptions;
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

        public Result<Project> CreateProject(CreateProjectOptions options)
        {
            if (options == null)
            {
                return Result<Project>
                    .CreateFailed(StatusCode.BadRequest, "Null options");
            }

            var user = userService.SearchUser(new SearchUserOptions()
            {
                UserId = options.UserId
            }).Include(u => u.Projects)
            .ThenInclude(p => p.Packages)
            .SingleOrDefault();

            if (user == null)
            {
                return Result<Project>
                    .CreateFailed(StatusCode.BadRequest, $"The UserId {options.UserId} does not exist");
            }

            if (IsValidProjectOptions(options))
            {
                Project project = new Project();

                project.Title = options.Title;

                project.Description = options.Description;

                project.Category = options.Category;

                project.FinancialGoal = options.FinancialGoal;

                foreach (var url in options.MediaUrls)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        project.Media.Add(new ProjectMedia()
                        {
                            MediaUrl = url
                        });
                    }
                }

                user.Projects.Add(project);
                user.IsProjectCreator = true;

                try
                {
                    if (dbContext.SaveChanges() > 0)
                    {
                        return Result<Project>
                        .CreateSuccessful(project);
                    }
                    else
                    {
                        return Result<Project>
                        .CreateFailed(StatusCode.InternalServerError, "Project could not be made");
                    }
                }
                catch (Exception ex)
                {
                    return Result<Project>
                    .CreateFailed(StatusCode.InternalServerError, ex.ToString());
                }

            }

            return Result<Project>
                        .CreateFailed(StatusCode.InternalServerError, "Project options are not valid");
        }

        public bool IsValidProjectOptions(CreateProjectOptions options)
        {
            if ((string.IsNullOrWhiteSpace(options.Title) ||
                string.IsNullOrWhiteSpace(options.Description) ||
                !Enum.IsDefined(typeof(ProjectCategory), options.Category)) ||
                options.MediaUrls.Count == 0 ||
                options.FinancialGoal <= 0m)
            {
                return false;
            }

            return true;
        }

        public Result<bool> DeleteProject(int id)
        {
            var result = new Result<bool>();

            if (id <= 0)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = $"Id {id} is invalid";
                return result;
            }

            var project = SearchProject(new SearchProjectOptions()
            {
                ProjectId = id
            }).Include(p => p.Packages)
                .Include(p => p.User)
                .ThenInclude(u => u.Projects)
                .SingleOrDefault();

            if (project == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"Project with id {id} was not found";
                return result;
            }

            try
            {
                dbContext.Remove(project);

                if (project.User.Projects.Count == 0)
                {
                    project.User.IsProjectCreator = false;
                }

                if (dbContext.SaveChanges() > 0)
                {
                    result.ErrorCode = StatusCode.OK;
                    result.Data = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = StatusCode.InternalServerError;
                result.ErrorText = ex.ToString();
                return result;
            }

            result.ErrorCode = StatusCode.InternalServerError;
            result.ErrorText = $"Customer could not be deleted";
            return result;
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

        public Result<bool> UpdateProject(UpdateProjectOptions options, int id)
        {
            var result = new Result<bool>();

            if (options == null)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = "Null options";
                return result;
            }

            var project = dbContext
                .Set<Project>()
                .Where(p => p.ProjectId == id)
                .Include(p => p.Packages)
                .Include(p => p.Media)
                .SingleOrDefault();

            if (project == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"Project with id {id} was not found";
                return result;
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

            if (options.MediaUrls != null)
            {
                project.Media.Clear();

                var optionsVideo = options.MediaUrls[3];
                options.MediaUrls.RemoveAt(3);

                foreach (var url in options.MediaUrls)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        project.Media.Add(new ProjectMedia()
                        {
                            MediaUrl = url
                        });
                    }
                }

                if (!string.IsNullOrWhiteSpace(optionsVideo))
                {
                    if (optionsVideo.Contains("youtube"))
                    {
                        project.Media.Add(new ProjectMedia()
                        {
                            MediaUrl = optionsVideo
                        });
                    }
                    else
                    {
                        result.ErrorCode = StatusCode.BadRequest;
                        result.ErrorText = $"Not a video from youtube";
                        return result;
                    }
                }
            }

            if (dbContext.SaveChanges() > 0)
            {
                result.ErrorCode = StatusCode.OK;
                result.Data = true;
                return result;
            }

            result.ErrorCode = StatusCode.InternalServerError;
            result.ErrorText = $"Project could not be updated";
            return result;
        }

        public Result<bool> CreateStatusUpdate(CreateProjectStatusOptions options, int id)
        {
            var result = new Result<bool>();

            if (options == null)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = "Null options";
                return result;
            }

            var project = dbContext
                .Set<Project>()
                .Where(p => p.ProjectId == id)
                .Include(p => p.StatusUpdates)
                .SingleOrDefault();

            if (project == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"Project with id {id} was not found";
                return result;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(options.StatusDescription))
                {
                    project.StatusUpdates.Add(new ProjectStatusUpdate()
                    {
                        Status = options.StatusDescription
                    });
                }
                if (dbContext.SaveChanges() > 0)
                {
                    result.ErrorCode = StatusCode.OK;
                    result.Data = true;
                    return result;
                }
            }
            catch(Exception ex)
            {
                result.ErrorCode = StatusCode.InternalServerError;
                result.ErrorText = ex.ToString();
                return result;
            }

            result.ErrorCode = StatusCode.InternalServerError;
            result.ErrorText = $"Status update could not be created";
            return result;
        }
    }
}
