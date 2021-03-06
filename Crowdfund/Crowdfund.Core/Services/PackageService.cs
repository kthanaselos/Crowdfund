﻿using Crowdfund.Core.Data;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crowdfund.Core.Services
{
    public class PackageService : IPackageService
    {
        private IProjectService projectService;
        private IUserService userService;
        private CrowdfundDbContext dbContext;

        public PackageService(IProjectService projectservice, IUserService uService, CrowdfundDbContext context)
        {
            this.projectService = projectservice;
            this.userService = uService;
            this.dbContext = context;
        }

        public Result<Package> CreatePackage(CreatePackageOptions options)
        {
            if (options == null)
            {
                return Result<Package>
                    .CreateFailed(StatusCode.BadRequest, "Null options");
            }

            var project = projectService.SearchProject(new SearchProjectOptions()
            {
                ProjectId = options.ProjectId
            }).SingleOrDefault();


            if (project == null)
            {
                return Result<Package>
                    .CreateFailed(StatusCode.BadRequest, $"The ProjectId {options.ProjectId} does not exist");
            }

            var package = new Package();

            if (!string.IsNullOrWhiteSpace(options.Description))
            {
                package.Description = options.Description;
            }
            else
            {
                return Result<Package>
                    .CreateFailed(StatusCode.BadRequest, $"Package description is empty");
            }

            if (options.Price > 0M)
            {
                package.Price = (decimal)options.Price;
            }
            else
            {
                return Result<Package>
                    .CreateFailed(StatusCode.BadRequest, $"Price is not valid");
            }

            project.Packages.Add(package);

            try
            {
                if (dbContext.SaveChanges() > 0)
                {
                    return Result<Package>
                    .CreateSuccessful(package);
                }
                else
                {
                    return Result<Package>
                    .CreateFailed(StatusCode.InternalServerError, "Package could not be created");
                }
            }
            catch (Exception ex)
            {
                return Result<Package>
                .CreateFailed(StatusCode.InternalServerError, ex.ToString());
            }
        }

        public Result<bool> DeletePackage(int packageId)
        {
            var result = new Result<bool>();

            if (packageId <= 0)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = $"Id {packageId} is invalid";
                return result;
            }

            var package = dbContext.Set<Package>()
                              .Where(p => p.PackageId == packageId)
                              .SingleOrDefault();

            if (package == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"Package with id {packageId} was not found";
                return result;
            }

            try
            {
                dbContext.Remove(dbContext.Set<Package>()
                                              .Where(p => p.PackageId == packageId)
                                              .SingleOrDefault());

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
            result.ErrorText = $"Package could not be deleted";
            return result;
        }

        public Result<bool> UpdatePackage(UpdatePackageOptions options, int id)
        {
            var result = new Result<bool>();

            if (options == null)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = "Null options";
                return result;
            }

            var package = dbContext.Set<Package>()
                .Where(p => p.PackageId == id)
                .SingleOrDefault();

            if (package == null) 
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"Package with id {id} was not found";
                return result;
            }

            if (!string.IsNullOrWhiteSpace(options.Description))
            {
                package.Description = options.Description;
            }

            if (dbContext.SaveChanges() > 0)
            {
                result.ErrorCode = StatusCode.OK;
                result.Data = true;
                return result;
            }

            result.ErrorCode = StatusCode.InternalServerError;
            result.ErrorText = $"Package could not be updated";
            return result;
        }

        public IQueryable<Package> SearchPackageById(int id)
        {
            var query = dbContext
                .Set<Package>()
                .Where(p => p.PackageId == id);

            return query;
        }
        public Result<bool> PurchasePackage(int packageId, int userId)
        {
            var result = new Result<bool>();

            var user = userService.SearchUser(new SearchUserOptions()
            {
                UserId = userId
            }).SingleOrDefault();

            if (user == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"User with id {userId} was not found";
                return result;
            }

            var package = SearchPackageById(packageId)
                .Include(p=>p.Project)
                .SingleOrDefault();

            if (package == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"Package with id {packageId} was not found";
                return result;
            }

            try
            {
                user.PurchasedPackages.Add(new PurchasedPackage()
                {
                    Package=package
                });

                //Update project's financial progress
                package.Project.FinancialProgress += package.Price;

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
            result.ErrorText = $"Package could not be purchased";
            return result;
        }
    }
}
