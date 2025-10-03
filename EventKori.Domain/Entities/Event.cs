using EventKori.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Entities
{
    public class Event : BaseEntity
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public int AttendeesCount { get; set; }
        public decimal Budget { get; set; }
        public string EventType { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Planning;
        public string SpecialRequirements { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
