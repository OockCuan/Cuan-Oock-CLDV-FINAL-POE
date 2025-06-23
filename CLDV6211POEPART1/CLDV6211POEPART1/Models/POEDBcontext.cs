using Microsoft.EntityFrameworkCore;

namespace CLDV6211POEPART1.Models
{
    public class POEDBcontext : DbContext
    {
        public POEDBcontext(DbContextOptions<POEDBcontext> options) :base(options) { 
            
        }

        public DbSet<Venue> Venue { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Booking> Booking { get; set; }
    }
}
