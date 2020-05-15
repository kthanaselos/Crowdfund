using System;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Model
{
    public class User
    {
        public int UserId { get; set; }
        public DateTime Created { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsProjectCreator { get; set; }
        public List<Project> Projects { get; set; }
        public List<PurchasedPackage> PurchasedPackages { get; set; }

        public User()
        {
            Created = DateTime.Now;
            IsProjectCreator = false;
            Projects = new List<Project>();
            PurchasedPackages = new List<PurchasedPackage>();
        }

    }
}
