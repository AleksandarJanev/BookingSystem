using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public class BookReq
    {
        public string OptionCode { get; set; }
        [Required]
        public SearchReq SearchReq { get; set; }
    }
}
