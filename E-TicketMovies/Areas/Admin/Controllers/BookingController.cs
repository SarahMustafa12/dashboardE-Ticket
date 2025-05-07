using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace E_TicketMovies.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBookingItemRepository bookingItemRepository;

        public BookingController(IBookingRepository bookingRepository,UserManager<ApplicationUser> userManager, IBookingItemRepository bookingItemRepository)
        {
            this.bookingRepository = bookingRepository;
            this.userManager = userManager;
            this.bookingItemRepository = bookingItemRepository;
        }
        public IActionResult Index(int? page = 1, string? query = null, bool? refunded = false , bool? booked = false , bool? cancelled = false)
        {
           var allBookings = bookingRepository.Get(includes: [e => e.ApplicationUser]); ;

            if (refunded == true)
            {
                allBookings = (List<Booking>)bookingRepository.Get(e => e.PaymentStripId == null, includes: [e => e.ApplicationUser]);
            }
            else if (booked == true)
            {
                allBookings = (List<Booking>)bookingRepository.Get(e => e.Status == true, includes: [e => e.ApplicationUser]);
            }
            else if (cancelled == true)
            {
                allBookings = (List<Booking>)bookingRepository.Get(e => e.Status == false && e.PaymentStatus == true , includes: [e => e.ApplicationUser]);
            }
            else 
            {
                allBookings = (List<Booking>)bookingRepository.Get(includes: [e => e.ApplicationUser]);

                if (!string.IsNullOrEmpty(query))

                    allBookings = allBookings.Where(b => b.ApplicationUser.UserName.Contains(query,StringComparison.OrdinalIgnoreCase)).ToList();
                
            }
            

            // Pagination
            int totalCount = allBookings.Count();
            int pageSize = 3;
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            if (page > totalPages && totalPages > 0)
                return RedirectToAction("NotFoundPage", "Home", new { area = "EndUser" });

            allBookings = allBookings.Skip(((int)page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.totalPages = totalPages;

            return View(allBookings.ToList());
        }

        public IActionResult Refund(int id)
        {

            var booking = bookingRepository.GetOne(e=>e.Id == id);

            if (booking != null)
            {
                if (booking.Status == false && booking.PaymentStripId != null)
                {
                    var service = new SessionService();
                    var session = service.Get(booking.SessionId);
                    var refundOptions = new RefundCreateOptions
                    {
                        PaymentIntent = booking.PaymentStripId,
                        Amount = (long)booking.TotalPrice,
                        Reason = RefundReasons.RequestedByCustomer
                    };
                    var refundService = new RefundService();
                    var refundSesstion = refundService.Create(refundOptions);

                    booking.PaymentStatus = false;
                    booking.PaymentStripId = null;
                    booking.Status = false;
                    bookingRepository.Commit();
                }
            }


            return RedirectToAction("Index", "Booking");

        }

       public IActionResult ShowDetails(int id)
        {

            var currrentUser = userManager.GetUserId(User);
            var booking = bookingRepository.GetOne(e => e.Id == id);
            var bookingItems = bookingItemRepository.Get(e => e.BookingId == id , includes: [e => e.Movie, e => e.Booking, e => e.Movie.Cinema]);
            ViewBag.Id = booking.Id;
            ViewBag.Total = booking.TotalPrice;
            ViewBag.Date = booking.BookingTime;
            ViewBag.User = booking.ApplicationUser;
            ViewBag.Status = booking.Status;
            return View(bookingItems.ToList());
        }
    }
}
