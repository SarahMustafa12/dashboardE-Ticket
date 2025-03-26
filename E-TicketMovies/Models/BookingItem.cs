namespace E_TicketMovies.Models
{
    public class BookingItem
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
    }
}
