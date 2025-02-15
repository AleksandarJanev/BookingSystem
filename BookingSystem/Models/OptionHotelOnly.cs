using System;
using System.Text.Json.Serialization;

namespace BookingSystem.Models
{
    public class OptionHotelOnly : Option
    {
        public OptionHotelOnly(): base()
        {
        }
        public int id { get; set; }
        public string destinationCode { get; set; }
        public int hotelCode { get; set; }
        public string hotelName { get; set; }
        public string price { get; set; }
    }
}