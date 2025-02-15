
using BookingSystem.Constants;
using BookingSystem.Models;
using System;
using System.Text.Json;

namespace BookingSystem.Strategy
{
    public class HotelSearch : ISearchStrategy
    {
        Random random = new Random();
        public async Task execSearchType(List<OptionHotelOnly> hotels, SearchRes searchRes, SearchReq req, HttpClient httpClient)
        {
            hotelOnlySearch(hotels, searchRes, random);

            if (!string.IsNullOrEmpty(req.DepartureAirport))
            {
                await FlightSearch.hotelAndFlightSearch(searchRes, httpClient, req);
            }
        }

        private async Task<SearchRes> hotelAndFlightSearch(SearchRes searchRes, HttpClient _httpClient, SearchReq req)
        {
            var hotelAndFlightResponse = await _httpClient.GetStringAsync($"{urls.FlightSearchUrl}?departureAirport={req.DepartureAirport}&arrivalAirport={req.Destination}");
            var flights = JsonSerializer.Deserialize<List<OptionHotelAndFlight>>(hotelAndFlightResponse);
            if (flights != null)
            {
                searchRes.Options.AddRange(flights);
            }
            return searchRes;
        }

        private void hotelOnlySearch(List<OptionHotelOnly> hotels, SearchRes searchRes, Random random)
        {
            setHotelPrice(hotels, false);
            searchRes.Options.AddRange(hotels);
        }

        private void setHotelPrice(List<OptionHotelOnly> hotels, bool lastMinute)
        {
            foreach (var hotel in hotels)
            {
                hotel.price = getHotelPrice(lastMinute);
            }
        }
        private string getHotelPrice(bool lastMinute)
        {
            // Last minute search - more expensive hotel price.
            int minPrice = lastMinute ? 200 : 20;
            int maxPrice = lastMinute ? 1001 : 501;
            return random.Next(minPrice, maxPrice).ToString() + " $";
        }

    }
}
