using BookingSystem.Constants;
using BookingSystem.Controllers;
using BookingSystem.Enums;
using BookingSystem.Factory;
using BookingSystem.IServices;
using BookingSystem.Models;
using BookingSystem.Strategy;
using System.Collections.Concurrent;
using System.IO.IsolatedStorage;
using System.Text.Json;

namespace BookingSystem.Services
{
    public class Manager : IManager
    {
        private static readonly ConcurrentDictionary<string, (BookRes Booking, int SleepTime, DateTime BookingTime, bool isWithin45Days)> _bookings = new();
        private readonly HttpClient _httpClient = new HttpClient();
        private static DateTime today = DateTime.Today;
        DateTime futureDate = today.AddDays(45);
        
        public async Task<SearchRes> Search(SearchReq req)
        {
            var searchRes = new SearchRes { Options = new List<Option>() };
            bool isWithin45Days = req.FromDate >= today && req.FromDate <= futureDate;
            var hotelResponse = await _httpClient.GetStringAsync($"{urls.HotelSearchUrl}?destinationCode={req.Destination}");

            var hotels = JsonSerializer.Deserialize<List<OptionHotelOnly>>(hotelResponse);

            if (hotels != null)
            {
                ISearchStrategy searchStrategy = isWithin45Days ? SearchFactory.GetSearch("lastminute", true) : SearchFactory.GetSearch("hotelonly", false);

                if (searchStrategy != null)
                {
                    await searchStrategy.execSearchType(hotels, searchRes, req, _httpClient);
                }
            }

            return searchRes;
        }

        public async Task<BookRes> Book(BookReq req)
        {
            var bookingCode = Guid.NewGuid().ToString("N").Substring(0, 6);
            var sleepTime = new Random().Next(30, 60);
            var bookTime = DateTime.Now;
            bool isWithin45Days = req.SearchReq.FromDate >= today && req.SearchReq.FromDate <= futureDate;
            BookRes newBooking = new BookRes(bookingCode, bookTime);

            _bookings[bookingCode] = (newBooking, sleepTime, DateTime.Now, isWithin45Days);

            // simulating small connection lag
            await Task.Run(async () =>
            {
                await Task.Delay(sleepTime * 60);
                _bookings[bookingCode] = (newBooking, sleepTime, DateTime.Now.AddSeconds(sleepTime), isWithin45Days);
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
                    if(data.isWithin45Days)
                    {
                        return new CheckStatusRes { Status = BookingStatusEnum.Failed };
                    }
                    else
                    {
                        return new CheckStatusRes { Status = BookingStatusEnum.Success };
                    }
                }
                return new CheckStatusRes { Status = BookingStatusEnum.Pending };
            }
            return new CheckStatusRes { Status = BookingStatusEnum.Failed };
        }

    }
}
