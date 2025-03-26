namespace E_TicketMovies.Models
{
    public class Booking
    { 
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public double TotalPrice { get; set; }
        public DateTime BookingTime { get; set; }

        public bool Status { get; set; }
        public bool PaymentStatus { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentStripId { get; set; }

    }
}
