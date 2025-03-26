using E_TicketMovies.Models;

namespace E_TicketMovies.Repositories.IRepositories
{
    public interface IBookingItemRepository : IRepository<BookingItem>
    {
        void CreateRange(List<BookingItem> bookingItems);
    }
}
