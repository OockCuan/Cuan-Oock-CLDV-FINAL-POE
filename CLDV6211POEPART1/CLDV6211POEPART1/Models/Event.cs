using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace CLDV6211POEPART1.Models
{
    public class Event
    {
        public int EventID { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        public int VenueID { get; set; }
        public DateTime EventDate { get; set; }
        public string EventDescription { get; set; }

        public Venue? Venue { get; set; }
        public List<Booking> Bookings { get; set; } = new();
        [NotMapped]
        //public int? EventTypeID { get; set; }
        //public EventType EventType { get; set; }

        public IFormFile? ImageFile { get; set; }
    
    }
}
