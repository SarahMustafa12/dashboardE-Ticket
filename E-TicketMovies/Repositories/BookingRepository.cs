using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;

namespace E_TicketMovies.Repositories
{
    public class BookingRepository : Repository<Booking> , IBookingRepository
    {
        private readonly ApplicationDbContext dbContex;

        public BookingRepository(ApplicationDbContext dbContex) : base(dbContex)
        {
            this.dbContex = dbContex;
        }
    }
}
