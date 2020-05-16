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
        public Package CreatePackage(CreatePackageOptions options)
        {
            throw new NotImplementedException();
        }

        public bool DeletePackage(int PackageId)
        {
            throw new NotImplementedException();
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
