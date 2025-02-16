using BookingSystem.Strategy;

namespace BookingSystem.Factory
{
    public class SearchFactory
    {
        public static ISearchStrategy GetSearch(string searchType, bool isWithin45Days) 
        {
            return (searchType.ToLower(), isWithin45Days) switch
            {
                ("hotelonly", false) => new HotelSearch(),
                ("lastminute", true) => new LastMinuteSearch()
            };
        }
    }
}
