using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventKori.Domain.Entities
{
    public class PortfolioItem : BaseEntity
    {
        public int ServiceProviderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public int AttendeesCount { get; set; }
        public decimal Budget { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
