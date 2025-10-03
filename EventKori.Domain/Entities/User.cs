using EventKori.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Entities
{
    public class User : BaseEntity
    {
        public string IdentityId { get; set; } // Links to ASP.NET Core Identity
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public UserType Type { get; set; } = UserType.Customer;

        // Navigation properties
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
