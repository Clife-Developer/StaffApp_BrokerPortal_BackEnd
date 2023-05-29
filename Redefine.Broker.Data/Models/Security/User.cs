using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redefine.Broker.Data.Models.Security
{
    public partial class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }

    }
}
