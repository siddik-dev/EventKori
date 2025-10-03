using Microsoft.AspNetCore.Identity;

namespace EventKori.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // This links to the Domain User entity
        public int? DomainUserId { get; set; }
    }
}
