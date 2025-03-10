using E_TicketMovies.Models;
using Microsoft.EntityFrameworkCore;

namespace E_TicketMovies.Data_Access
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {

        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<ActorMovie>ActorMovies { get; set; }
        public ApplicationDbContext()
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ETicket;Integrated Security=True;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ActorMovie>().HasKey(e => new {e.MovieId,e.ActorId});
        }

    }
}
