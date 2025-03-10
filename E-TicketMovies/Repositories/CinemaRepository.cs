using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;

namespace E_TicketMovies.Repositories
{
    public class CinemaRepository : Repository<Cinema>, ICinemaRepository
    {
        private readonly ApplicationDbContext dbContex;

        public CinemaRepository(ApplicationDbContext dbContex) : base(dbContex)
        {
            this.dbContex = dbContex;
        }

    }
}
