using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;

namespace E_TicketMovies.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContex;

        public CategoryRepository(ApplicationDbContext dbContex) :base(dbContex) 
        {
            this.dbContex = dbContex;
        }
    }
}
