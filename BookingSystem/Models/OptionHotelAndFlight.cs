using System.Text.Json.Serialization;

namespace BookingSystem.Models
{
    public class OptionHotelAndFlight : Option
    {
        public OptionHotelAndFlight() : base()
        {
        }

        public int flightCode { get; set; }
        public string flightNumber { get; set; }
        public string departureAirport { get; set; }
        public string arrivalAirport { get; set; }

    }
}
