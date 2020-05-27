using Crowdfund.Core.Data;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Crowdfund.Core.Services
{
    public class UserService : IUserService
    {
        private CrowdfundDbContext dbContext;

        public UserService(CrowdfundDbContext context)
        {
            dbContext = context;
        }
        public Result<User> CreateUser(CreateUserOptions options)
        {
            if (options == null)
            {
                return Result<User>
                    .CreateFailed(StatusCode.BadRequest, "Null options");
            }

            var user = new User();

            if (!string.IsNullOrWhiteSpace(options.FirstName))
            {
                user.FirstName = options.FirstName;
            }
            else
            {
                return Result<User>
                    .CreateFailed(StatusCode.BadRequest, "First name was null or empty");
            }

            if (!string.IsNullOrWhiteSpace(options.LastName))
            {
                user.LastName = options.LastName;
            }
            else
            {
                return Result<User>
                    .CreateFailed(StatusCode.BadRequest, "Last name was null or empty");
            }

            if (string.IsNullOrWhiteSpace(options.Email))
            {
                return Result<User>
                    .CreateFailed(StatusCode.BadRequest, "Email was null or empty");
            }

            options.Email = options.Email.Trim();

            if (IsValidEmail(options.Email))
            {
                user.Email = options.Email;
            }
            else
            {
                return Result<User>
                    .CreateFailed(StatusCode.BadRequest, "Email submitted was not a valid email address");
            }

            dbContext.Add(user);

            try
            {
                if (dbContext.SaveChanges() > 0)
                {
                    return Result<User>
                    .CreateSuccessful(user);
                }
                else
                {
                    return Result<User>
                    .CreateFailed(StatusCode.InternalServerError, "User could not be created");

                }

            }
            catch (Exception ex)
            {
                return Result<User>
                    .CreateFailed(StatusCode.InternalServerError, ex.ToString());
            }
        }

        public IQueryable<User> SearchUser(SearchUserOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var query = dbContext
                .Set<User>()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(options.FirstName))
            {
                query = query.Where(u => u.FirstName == options.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(options.LastName))
            {
                query = query.Where(u => u.LastName == options.LastName);
            }

            if (!string.IsNullOrWhiteSpace(options.Email))
            {
                query = query.Where(u => u.Email == options.Email);
            }

            if (options.CreatedFrom != default(DateTime))
            {
                query = query.Where(u => u.Created >= options.CreatedFrom);
            }

            if (options.CreatedTo != default(DateTime))
            {
                query = query.Where(u => u.Created <= options.CreatedTo);
            }

            if (options.UserId != null)
            {
                query = query.Where(u => u.UserId == options.UserId.Value);
            }

            if (options.IsProjectCreator != null)
            {
                query = query.Where(u => u.IsProjectCreator == options.IsProjectCreator.Value);
            }

            return query;
        }

        public User GetUserById(int id)
        {
            var user = SearchUser(new SearchUserOptions() { UserId = id })
                .Include(u => u.Projects)
                .ThenInclude(p => p.Packages)
                .SingleOrDefault();

            if (user == null)
            {
                return null;
            }
            return user;
        }

        public Result<bool> UpdateUser(UpdateUserOptions options, int id)
        {
            var result = new Result<bool>();

            if (options == null)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = "Null options";
                return result;
            }

            var user = GetUserById(id);

            if (user == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"User with id {id} was not found";
                return result;
            }

            if (options.FirstName != null)
            {
                user.FirstName = options.FirstName;
            }
            if (options.LastName != null)
            {
                user.LastName = options.LastName;
            }
            if (options.Email != null)
            {
                if (string.IsNullOrWhiteSpace(options.Email))
                {
                    result.ErrorCode = StatusCode.BadRequest;
                    result.ErrorText = $"Email address is null or empty";
                    return result;
                }

                //options.Email = options.Email.Trim();

                //if (options.Email.Contains("@") && options.Email.EndsWith(".com"))
                //{
                //    user.Email = options.Email;
                //}
                //else
                //{
                //    result.ErrorCode = StatusCode.BadRequest;
                //    result.ErrorText = $"Email submitted is not a valid email address";
                //    return result;
                //}

                if (IsValidEmail(options.Email))
                {
                    user.Email = options.Email.Trim();
                }
                else
                {
                    result.ErrorCode = StatusCode.BadRequest;
                    result.ErrorText = $"Email submitted is not a valid email address";
                    return result;
                }

            }

            if (dbContext.SaveChanges() > 0)
            {
                result.ErrorCode = StatusCode.OK;
                result.Data = true;
                return result;
            }

            result.ErrorCode = StatusCode.InternalServerError;
            result.ErrorText = $"User could not be updated";
            return result;
        }

        bool IsValidEmail(string mail)
        {
            Regex mRegxExpression;
            if (mail.Trim() != string.Empty)
            { 
                mRegxExpression = new Regex(@"^([a-zA-Z0-9_\-])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$");

                if (!mRegxExpression.IsMatch(mail.Trim()))
                {
                    return false;
                }
            }
            return true;
        }

        public Result<bool> DeleteUser(int id)
        {
            var result = new Result<bool>();

            if (id <= 0)
            {
                result.ErrorCode = StatusCode.BadRequest;
                result.ErrorText = $"Id {id} is invalid";
                return result;
            }

            var user = GetUserById(id);

            if (user == null)
            {
                result.ErrorCode = StatusCode.NotFound;
                result.ErrorText = $"User with id {id} was not found";
                return result;
            }

            try
            {
                dbContext.Remove(user);
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
            result.ErrorText = $"User could not be deleted";
            return result;
        }
    }
}