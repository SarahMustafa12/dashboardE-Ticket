using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;

namespace E_TicketMovies.Repositories
{
    public class ActorMovieRepository : Repository<ActorMovie>, IActorMovieRepository
    {
        private readonly ApplicationDbContext dbContex;
        public ActorMovieRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContex = dbContex;
        }
    }
}
