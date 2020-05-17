using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crowdfund.Core.Services
{
    public interface IUserService
    {
        User CreateUser(CreateUserOptions options);
        IQueryable<User> SearchUser(SearchUserOptions options);
        User UpdateUser(UpdateUserOptions options,int id);
        User GetUserById(int id);
        bool DeleteUser(int id);
    }
}
