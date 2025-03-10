using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;

namespace E_TicketMovies.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext dbContex;

        public MovieRepository(ApplicationDbContext dbContex) : base(dbContex)
        {
            this.dbContex = dbContex;
        }
    }
}
