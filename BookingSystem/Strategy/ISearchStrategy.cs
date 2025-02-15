using BookingSystem.Models;

namespace BookingSystem.Strategy
{
    public interface ISearchStrategy
    {
        Task execSearchType(List<OptionHotelOnly> hotels, SearchRes searchRes, SearchReq req, HttpClient httpClient);
    }
}
