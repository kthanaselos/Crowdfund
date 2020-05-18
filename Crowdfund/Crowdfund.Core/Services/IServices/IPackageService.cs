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
        bool UpdatePackage(UpdatePackageOptions options, int id);
        bool DeletePackage(int packageId);
        
    }
}
