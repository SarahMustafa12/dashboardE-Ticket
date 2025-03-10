using System.Diagnostics;
using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_TicketMovies.Controllers
{
    [Area("End User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext dbContext = new ApplicationDbContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index(string search)
        {
            IQueryable<Movie> movies = dbContext.Movies.Include(e=>e.Category).Include(e=>e.Cinema);
            if (search!= null)
            {
                movies = movies.Where(e=> e.Category.Name == search || e.Name.Contains(search));   
            }
            
            return View(movies.ToList());
        }
        public IActionResult ShowCinema()
        {
            var cinema = dbContext.Cinemas.ToList();
            return View(cinema);
        }
        public IActionResult ShowDetails(int id)
        {
            var movies = dbContext.Movies.Include(e => e.Category).Include(e => e.Cinema).Include(e => e.Actors).ThenInclude(e => e.Actor).FirstOrDefault(e => e.Id == id);

         
            return View(movies);
        }

        public IActionResult ActorDetails(int movieId, int actorId)
        {
            var actor = dbContext.ActorMovies.FirstOrDefault(e => e.ActorId == actorId && e.MovieId == movieId);

            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        public IActionResult ShowCategory()
        {
            var categories = dbContext.Categories.Include(e => e.Movies);
            return View(categories.ToList());
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
