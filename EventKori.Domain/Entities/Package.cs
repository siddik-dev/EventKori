using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Entities
{
    public class Package : BaseEntity
    {
        public int ServiceProviderId { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Features { get; set; } // JSON serialized features
        public int DurationHours { get; set; }
        public bool IsPopular { get; set; }
        public int MaxAttendees { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
