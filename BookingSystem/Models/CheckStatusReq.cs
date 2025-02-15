using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Models
{
    public class CheckStatusReq
    {
        [Required]
        public string BookingCode { get; set; }
    }
}
