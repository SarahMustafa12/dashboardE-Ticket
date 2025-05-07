using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace E_TicketMovies.Areas.End_User.Controllers
{
    [Area("End User")]
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingItemRepository bookingItemRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public BookingController(IBookingRepository bookingRepository , IBookingItemRepository bookingItemRepository, UserManager<ApplicationUser> userManager)
        {
            this.bookingRepository = bookingRepository;
            this.bookingItemRepository = bookingItemRepository;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var currrentUser = userManager.GetUserId(User);

            var allbookings = bookingRepository.Get(e=>e.ApplicationUserId == currrentUser);

            return View(allbookings.ToList());
        }
        public IActionResult BookingDetalis(int id)
        {
            var currrentUser = userManager.GetUserId(User);
            var booking = bookingRepository.GetOne(e=>e.Id == id && currrentUser == e.ApplicationUserId);
            var bookingItems = bookingItemRepository.Get(e=>e.BookingId == id && currrentUser == e.Booking.ApplicationUserId , includes:[e =>e.Movie, e => e.Booking , e =>e.Movie.Cinema]);
            ViewBag.Id = booking.Id;
            ViewBag.Total = booking.TotalPrice;
            ViewBag.Date = booking.BookingTime;
            ViewBag.User = booking.ApplicationUser;
            ViewBag.Status = booking.Status;
               return View(bookingItems.ToList());
        }

        public IActionResult Canceled(int id)
        {
            var currrentUser = userManager.GetUserId(User);
            var booking = bookingRepository.GetOne(e => e.Id == id && currrentUser == e.ApplicationUserId);
            if(booking != null)
            {

                if (booking.Status == true && booking.PaymentStripId != null)
                {
                    booking.Status = false; // canceled 
                    booking.PaymentStatus = true; 
                    bookingRepository.Update(booking);  
                    bookingRepository.Commit(); 
                }
            }
            return RedirectToAction("Index", "Booking", new { area = "End User" });

        }
       
        
    }
}
