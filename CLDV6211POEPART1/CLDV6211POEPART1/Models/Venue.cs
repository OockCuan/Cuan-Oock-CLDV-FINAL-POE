using System.ComponentModel.DataAnnotations.Schema;

namespace CLDV6211POEPART1.Models
{
    public class Venue
    {

        //gets and sets for every attribute for the venue entity
        public int VenueID { get; set; }

        public string? VenueName { get; set; }
        
        public string? VenueLocation { get; set; }
        public int Capacity { get; set; }

        public string? ImageURL { get; set; }

        [NotMapped]

        public IFormFile? ImageFile { get; set; }
    }
}
