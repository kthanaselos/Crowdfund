using Crowdfund.Core.Model;
using Crowdfund.Core.Services.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crowdfund.Core.Services
{
    public interface IPackageService
    {
        Package CreatePackage(CreatePackageOptions options);
        IQueryable<Package> SearchUser(SearchPackageOptions options);
        Package UpdatePackage(UpdatePackageOptions options);
        bool DeletePackage(int PackageId);
        
    }
}
