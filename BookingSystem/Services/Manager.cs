using BookingSystem.Controllers;
using BookingSystem.Enums;
using BookingSystem.IServices;
using BookingSystem.Models;
using System.Collections.Concurrent;
using System.IO.IsolatedStorage;
using System.Text.Json;

namespace BookingSystem.Services
{
    public class Manager : IManager
    {
        private static readonly ConcurrentDictionary<string, (BookRes Booking, int SleepTime, DateTime BookingTime)> _bookings = new();
        private readonly HttpClient _httpClient = new HttpClient();
        private static DateTime today = DateTime.Today;
        DateTime futureDate = today.AddDays(45);
        Random random = new Random();
        //private readonly HttpClient _httpClient;
        //private readonly BookController _bookController;
        //private readonly SearchController _searchController;


        // ADD FACTORY FOR HOTELONLY, LASTMINUTEHOTEL AND FLIGHTANDHOTELSEARCH 
        public async Task<SearchRes> Search(SearchReq req)
        {
            var searchRes = new SearchRes { Options = new List<Option>() };
            bool isWithin45Days = req.FromDate >= today && req.FromDate <= futureDate;

            var hotelResponse = await _httpClient.GetStringAsync($"https://tripx-test-functions.azurewebsites.net/api/SearchHotels?destinationCode={req.Destination}");
            var hotels = JsonSerializer.Deserialize<List<OptionHotelOnly>>(hotelResponse);

            if(isWithin45Days && hotels != null)
            {
                // Last minute search - more expensive hotel price.
                foreach (var hotel in hotels)
                {
                    hotel.price = (random.Next(200, 1001)).ToString() + " $";
                }

                searchRes.Options.AddRange(hotels);
                return searchRes;
            }
            
            // Hotel Only Search
            if (hotels != null)
            {
                foreach (var hotel in hotels)
                {
                    hotel.price = (random.Next(20, 501)).ToString() + " $";
                }

                searchRes.Options.AddRange(hotels);
            }

            // Hotel and Flight Search
            if(!string.IsNullOrEmpty(req.DepartureAirport))
            {
                var hotelAndFlightResponse = await _httpClient.GetStringAsync($"https://tripx-test-functions.azurewebsites.net/api/SearchFlights?departureAirport={req.DepartureAirport}&arrivalAirport={req.Destination}");
                var flights = JsonSerializer.Deserialize<List<OptionHotelAndFlight>>(hotelAndFlightResponse);
                if(flights != null)
                {
                    searchRes.Options.AddRange(flights);
                }
            }

            return searchRes;
        }

        public async Task<BookRes> Book(BookReq req)
        {
            var bookingCode = Guid.NewGuid().ToString("N").Substring(0, 6);
            var sleepTime = new Random().Next(30, 60);
            var bookTime = DateTime.Now;

            BookRes newBooking = new BookRes (bookingCode, bookTime);

            _bookings[bookingCode] = (newBooking,  sleepTime, DateTime.Now);

            // simulating small connection lag
            await Task.Run(async () =>
            {
                await Task.Delay(sleepTime * 60);
                 _bookings[bookingCode] = (newBooking, sleepTime, DateTime.Now.AddSeconds(sleepTime));
            });

            return newBooking;
        }

        public async Task<CheckStatusRes> CheckStatus(CheckStatusReq req)
        {
            if (_bookings.TryGetValue(req.BookingCode, out var data))
            {
                var bookingTimeSpent = (DateTime.Now - data.BookingTime).TotalSeconds;
              
                if (bookingTimeSpent >= data.SleepTime)
                {
                    return new CheckStatusRes { Status = BookingStatusEnum.Success };
                }
                return new CheckStatusRes { Status = BookingStatusEnum.Pending };
            }
            return new CheckStatusRes { Status = BookingStatusEnum.Failed };
        }
    }
}
