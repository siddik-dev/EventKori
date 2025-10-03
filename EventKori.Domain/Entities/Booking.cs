using EventKori.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public int EventId { get; set; }
        public int ServiceProviderId { get; set; }
        public int? PricingPackageId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public DateTime EventDate { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string SpecialRequirements { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string Notes { get; set; }

        // Navigation properties
        public virtual Event Event { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual Package Package { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
