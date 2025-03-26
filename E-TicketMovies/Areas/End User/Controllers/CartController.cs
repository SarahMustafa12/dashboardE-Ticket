using E_TicketMovies.Data_Access;
using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_TicketMovies.Areas.End_User.Controllers
{
    [Area("End User")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly IMovieRepository movieRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingItemRepository bookingItemRepository;

        public CartController(ICartRepository cartRepository ,IMovieRepository movieRepository, UserManager<ApplicationUser> userManager,IBookingRepository bookingRepository, IBookingItemRepository bookingItemRepository)
        {
            this.cartRepository = cartRepository;
            this.movieRepository = movieRepository;
            this.userManager = userManager;
            this.bookingRepository = bookingRepository;
            this.bookingItemRepository = bookingItemRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddToCart()
        {
            var currentUser = userManager.GetUserId(User);
            var itemsInCart = cartRepository.Get(e => e.ApplicationUserId == currentUser, includes: [e => e.Movie, e => e.Movie.Cinema]);
            var totalPrie = itemsInCart.Sum(e=>e.Movie.Price * e.Count);
            
            ViewBag.TotalPrie = totalPrie;

            return View(itemsInCart.ToList());
        }
        public IActionResult Increment(int movieId)
        {
            var targetMovie = cartRepository.GetOne(e=>e.MovieId == movieId && userManager.GetUserId(User) == e.ApplicationUserId);
            if (targetMovie != null)
            {
                targetMovie.Count++;
                cartRepository.Update(targetMovie);
                cartRepository.Commit();
            }
            return RedirectToAction(nameof(AddToCart));
        }

        public IActionResult Decrement(int movieId)
        {
            var targetMovie = cartRepository.GetOne(e => e.MovieId == movieId && userManager.GetUserId(User) == e.ApplicationUserId);
            if (targetMovie != null)
            {
                if (targetMovie.Count > 1)
                { 
                targetMovie.Count--;
                cartRepository.Update(targetMovie);
                cartRepository.Commit();
                }
            }
            return RedirectToAction(nameof(AddToCart));
        }
        public IActionResult Delete(int movieId) {

            var targetMovie = cartRepository.GetOne(e=>e.MovieId == movieId && userManager.GetUserId(User)==e.ApplicationUserId);
            if (targetMovie != null) { 

                cartRepository.Delete(targetMovie);
                cartRepository.Commit();
            }
            return RedirectToAction(nameof(AddToCart));
        }
        public IActionResult Checkout()
        {
            var currentUser = userManager.GetUserId(User);
            var itemsInCart = cartRepository.Get(e => e.ApplicationUserId == currentUser, includes: [e => e.Movie, e => e.Movie.Cinema]);
            var booking = new Booking();
            
            booking.ApplicationUserId = currentUser;
            booking.BookingTime = DateTime.Now; 
            booking.TotalPrice =(long)itemsInCart.Sum(e => e.Movie.Price * e.Count);

            bookingRepository.Create(booking);
            bookingRepository.Commit();
            

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/End%20User/Checkout/Success?bookingId={booking.Id}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/End%20User/Checkout/Cancel",
            };
            foreach (var item in itemsInCart)
            { 
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency= "egp",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Movie.Name,
                        Description = item.Movie.Cinema.Name,

                    },
                    UnitAmount =(long)item.Movie.Price*100,
                },
                Quantity = item.Count
                
            });
            }
            var service = new SessionService();
            var session = service.Create(options);
            booking.SessionId = session.Id;
            bookingRepository.Commit();
            List<BookingItem> bookingItems = [];
            foreach (var item in itemsInCart) 
            {
                var bookingItem = new BookingItem()
                {
                    BookingId = booking.Id,
                    MovieId = item.MovieId,
                    Count = item.Count,
                    Price = (long)item.Movie.Price
                };

                bookingItems.Add(bookingItem);
            }

            bookingItemRepository.CreateRange(bookingItems);
            bookingRepository.Commit();
            
            return Redirect(session.Url);
            
        }
       
    }
}
