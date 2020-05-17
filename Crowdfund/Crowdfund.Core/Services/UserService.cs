using Crowdfund.Core.Data;
using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crowdfund.Core.Services
{
    public class UserService : IUserService
    {
        private CrowdfundDbContext dbContext;

        public UserService(CrowdfundDbContext context)
        {
            dbContext = context;
        }
        public User CreateUser(CreateUserOptions options)
        {
            if (options == null)
            {
                return null;
            }

            var user = new User()
            {
                FirstName = options.FirstName,
                LastName = options.LastName,
                Email = options.Email
            };

            dbContext.Add(user);

            if (dbContext.SaveChanges() > 0)
            {
                return user;
            }

            return null;
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
                .Include(u=>u.Projects)
                .ThenInclude(p=>p.Packages)
                .SingleOrDefault();

            if (user == null)
            {
                return null;
            }
            return user;
        }

        public User UpdateUser(UpdateUserOptions options,int id)
        {
            var user = GetUserById(id);

            if (options == null || user == null)
            {
                return null;
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
                user.Email = options.Email;
            }

            if (dbContext.SaveChanges() > 0)
            {
                return user;
            }

            return null;
        }

        public bool DeleteUser(int id)
        {
            var user = GetUserById(id);
            dbContext.Remove(user);
            if (dbContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
