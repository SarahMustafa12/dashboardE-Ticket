using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index(int? page = 1, string? query = null, bool? failed = false)
        {
            List<Booking> allBookings;

            if (failed == true)
            {
                // Get failed bookings (where PaymentStripId is null)
                allBookings = (List<Booking>)bookingRepository.Get(e => e.PaymentStripId == null, includes: [e => e.ApplicationUser]);
            }
            else
            {
                allBookings = (List<Booking>)bookingRepository.Get(includes: [e => e.ApplicationUser]);

                // Optional: Add search filter if query is provided
                if (!string.IsNullOrEmpty(query))
                {
                    allBookings = allBookings.Where(b => b.ApplicationUser.UserName.Contains(query)).ToList();
                }
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

        //public IActionResult Index(int query, int page, List<Booking> failed)
        //{
        //    var allBookings = bookingRepository.Get(includes: [e=>e.ApplicationUser]);

        //    failed = bookingRepository.Get(e=>e.PaymentStripId == null ,includes: [e => e.ApplicationUser]).ToList();


        //    //if (allBookings != null)
        //    //{
        //    //    allBookings = bookingRepository.Get(e => e.Id == query);

        //    //}
        //    int totalCount = allBookings.Count();
        //    int pageSize = 3;
        //    int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        //    if (page > totalPages && totalPages > 0)
        //        return RedirectToAction("NotFoundPage", "Home", new { area = "End User" });

        //    allBookings = allBookings.Skip((page - 1) * pageSize).Take(pageSize);

        //    ViewBag.totalPages = totalPages;

        //    return View(allBookings.ToList());
        //}
    }
}
