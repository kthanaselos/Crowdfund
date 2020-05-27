using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crowdfund.Core.Services
{
    public interface IProjectService
    {
        Result<Project> CreateProject(CreateProjectOptions options);
        IQueryable<Project> SearchProject(SearchProjectOptions options);
        Result<bool> UpdateProject(UpdateProjectOptions options,int id);
        Result<bool> DeleteProject(int id);
    }
}
