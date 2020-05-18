using Crowdfund.Core.Data;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crowdfund.Core.Services
{
    public class PackageService : IPackageService
    {
        private IProjectService projectService;
        private CrowdfundDbContext dbContext;

        public PackageService(IProjectService projectservice, CrowdfundDbContext context)
        {
            this.projectService = projectservice;
            this.dbContext = context;
        }

        public Package CreatePackage(CreatePackageOptions options)
        {
            if (options == null || options.ProjectId == null)
            {
                return null;
            }

            var project = dbContext
                .Set<Project>()
                .Where(p => p.ProjectId == options.ProjectId)
                .SingleOrDefault();

            if (project == null)
            {
                return null;
            }

            var package = new Package();

            if (!string.IsNullOrWhiteSpace(options.Description))
            {
                package.Description = options.Description;
            }

            if (options.Price > 0M)
            {
                package.Price = (decimal)options.Price;
            }

            project.Packages.Add(package);
            return dbContext.SaveChanges() > 0 ? package : null;
        }

        public bool DeletePackage(int PackageId)
        {
            //dbContext.Remove(dbContext.Set<Package>().Find(PackageId));

            try
            {
                dbContext.Remove(dbContext.Set<Package>()
                                              .Where(p => p.PackageId == PackageId)
                                              .SingleOrDefault());
            }
            catch (Exception ex)
            {
                return false;
            }

            if (dbContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        public IQueryable<Package> SearchUser(SearchPackageOptions options)
        {
            throw new NotImplementedException();
        }

        public Package UpdatePackage(UpdatePackageOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
