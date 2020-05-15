using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Model
{
    public class PurchasedPackage
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public User User { get; set; }
        public Package Package { get; set; }
    }
}
