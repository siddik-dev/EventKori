namespace EventKori.Domain.Entities
{
    public class ServiceProvider : BaseEntity
    {
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string ServicesOffered { get; set; }
        public decimal StartingPrice { get; set; }
        public string Location { get; set; }
        public decimal Rating { get; set; }
        public int TotalReviews { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsVerified { get; set; }
        public string BusinessLicense { get; set; }
        public string TaxId { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<PortfolioItem> PortfolioItems { get; set; } = new List<PortfolioItem>();
        public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
