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
        Result<User> CreateUser(CreateUserOptions options);
        IQueryable<User> SearchUser(SearchUserOptions options);
        Result<bool> UpdateUser(UpdateUserOptions options,int id);
        User GetUserById(int id);
        Result<bool> DeleteUser(int id);
    }
}
