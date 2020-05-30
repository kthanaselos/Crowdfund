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
        Result<Package> CreatePackage(CreatePackageOptions options);
        Result<bool> UpdatePackage(UpdatePackageOptions options, int id);
        Result<bool> DeletePackage(int packageId);
        
    }
}
