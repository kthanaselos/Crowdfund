using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Crowdfund.Core.Services.Options
{
    public class SearchUserOptions
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? IsProjectCreator { get; set; }        
        public DateTime CreatedFrom { get; private set; }
        public DateTime CreatedTo { get; private set; }
        
    }
}
