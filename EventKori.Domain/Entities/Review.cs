using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int BookingId { get; set; }
        public int ServiceProviderId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
        public string Response { get; set; } // Service provider's response
        public DateTime? ResponseDate { get; set; }

        // Navigation properties
        public virtual Booking Booking { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual User User { get; set; }
    }
}
