using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;

namespace E_TicketMovies.Repositories
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        private readonly ApplicationDbContext dbContex;

        public ActorRepository(ApplicationDbContext dbContex) : base(dbContex)
        {
            this.dbContex = dbContex;
        }
    }
}
