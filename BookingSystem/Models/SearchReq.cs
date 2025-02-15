using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public class SearchReq
    {
        [Required]
        public string Destination { get; set; }
        public string DepartureAirport { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
    }
}
