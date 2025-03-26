using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;

namespace E_TicketMovies.Repositories
{
    public class BookingItemRepository : Repository<BookingItem> , IBookingItemRepository
    {
        private readonly ApplicationDbContext dbContex;

        public BookingItemRepository(ApplicationDbContext dbContex) : base(dbContex)
        {
            this.dbContex = dbContex;
        }

        public void CreateRange(List<BookingItem> bookingItems)
        {
            dbContex.AddRange(bookingItems);
        }
    }
}
