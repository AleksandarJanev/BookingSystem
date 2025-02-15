using BookingSystem.Constants;
using BookingSystem.Models;
using System.Text.Json;

namespace BookingSystem.Strategy
{
    public class LastMinuteSearch : ISearchStrategy
    {
        Random random = new Random();
        public async Task execSearchType(List<OptionHotelOnly> hotels, SearchRes searchRes, SearchReq req, HttpClient httpClient)
        {
            lastMinuteSearch(hotels, searchRes, random);

            if (!string.IsNullOrEmpty(req.DepartureAirport))
            {
                await FlightSearch.hotelAndFlightSearch(searchRes, httpClient, req);
            }
        }

        private SearchRes lastMinuteSearch(List<OptionHotelOnly> hotels, SearchRes searchRes, Random random)
        {
            setHotelPrice(hotels, true);
            searchRes.Options.AddRange(hotels);
            return searchRes;
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
