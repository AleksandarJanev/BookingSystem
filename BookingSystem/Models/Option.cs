using System.Text.Json.Serialization;

namespace BookingSystem.Models
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(OptionHotelOnly))]
    [JsonDerivedType(typeof(OptionHotelAndFlight))]
    public abstract class Option
    {
        protected Option() { }
    }
}
