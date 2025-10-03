using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // This links to the Domain User entity
        public int? DomainUserId { get; set; }
    }
}
