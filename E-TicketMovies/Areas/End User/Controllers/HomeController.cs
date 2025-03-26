using System.Diagnostics;
using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_TicketMovies.Controllers
{
    [Area("End User")]
    public class HomeController :  Controller

    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ICartRepository cartRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICinemaRepository cinemaRepository;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, ICartRepository cartRepository, ICategoryRepository categoryRepository, ICinemaRepository cinemaRepository)
        {
            _logger = logger;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.cartRepository = cartRepository;
            this.categoryRepository = categoryRepository;
            this.cinemaRepository = cinemaRepository;
        }

       
        [HttpGet]
        public IActionResult Index(string? categoryName, string? cinemaName, string? movieName)
        {
            var currentUser = userManager.GetUserId(User);
            IQueryable<Movie> movies = dbContext.Movies.Include(e => e.Category).Include(e => e.Cinema);
            var itemsInCart = cartRepository.Get(e => e.ApplicationUserId == currentUser, includes: [e => e.Movie, e => e.Movie.Cinema]);
            var itemsNum = itemsInCart.Sum(e => e.Count);
            ViewBag.Count = itemsNum;

            var categories = categoryRepository.Get();
            var allcinema = cinemaRepository.Get();   
            ViewBag.categories = categories;
            ViewBag.Cinemas = allcinema;

            if (movieName != null)
            {
                movies = movies.Where(e => e.Name.Contains(movieName));
            }
            if (categoryName != null)
            {
                movies = movies.Where(e => e.Category.Name == categoryName);
            }
            if (cinemaName != null)
            {
                movies = movies.Where(e => e.Cinema.Name == cinemaName);
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
            var actor = dbContext.ActorMovies.Include(e => e.Actor).Include(e => e.Movie).FirstOrDefault(e => e.ActorId == actorId && e.MovieId == movieId);

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
        [HttpPost]
        // when i click on button book now would add the movie to the cart
        [Authorize]
        public IActionResult BookTicket(int movieId)
        {
            var currentUser = userManager.GetUserId(User);
            var target = dbContext.Movies.FirstOrDefault(e => e.Id == movieId).Id;
            var moviesInCart = dbContext.Carts.Where(e => e.MovieId == movieId).AsNoTracking().FirstOrDefault();
            if (moviesInCart != null)
            {

                if (moviesInCart.MovieId == target && currentUser == moviesInCart.ApplicationUserId)
                {
                    moviesInCart.Count++;
                    dbContext.Carts.Update(moviesInCart);
                    dbContext.SaveChanges();

                }
            }
            else
            {
                var cart = new Cart
                {
                    ApplicationUserId = currentUser,
                    MovieId = target,
                };
                cart.Count++;
                dbContext.Carts.Add(cart);
                dbContext.SaveChanges();
            }
        
            int numItemsInTheCart = dbContext.Carts.Count();
            ViewData[nameof(numItemsInTheCart)] = numItemsInTheCart;
            return RedirectToAction("Index", "Home", new {area = "End User"});  
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
