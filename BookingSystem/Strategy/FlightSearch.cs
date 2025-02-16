using BookingSystem.Constants;
using BookingSystem.Models;
using System.Text.Json;

namespace BookingSystem.Strategy
{
    public class FlightSearch
    {
        public static async Task<SearchRes> flightSearch(SearchRes searchRes, HttpClient _httpClient, SearchReq req)
        {
            var hotelAndFlightResponse = await _httpClient.GetStringAsync($"{urls.FlightSearchUrl}?departureAirport={req.DepartureAirport}&arrivalAirport={req.Destination}");
            var flights = JsonSerializer.Deserialize<List<OptionHotelAndFlight>>(hotelAndFlightResponse);
            if (flights != null)
            {
                searchRes.Options.AddRange(flights);
            }
            return searchRes;
        }
    }
}
