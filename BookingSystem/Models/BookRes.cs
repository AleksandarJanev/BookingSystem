namespace BookingSystem.Models
{
    public class BookRes
    {
        public BookRes(string bookingCode, DateTime bookingTime)
        {
            BookingCode = bookingCode;
            BookingTime = bookingTime;
        }

        public string BookingCode { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
