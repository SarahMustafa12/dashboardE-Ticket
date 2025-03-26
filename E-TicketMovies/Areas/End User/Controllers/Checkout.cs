using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe.Climate;

namespace E_TicketMovies.Areas.End_User.Controllers
{
    [Area("End User")]
    public class Checkout : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBookingItemRepository bookingItemRepository;

        public Checkout(IBookingRepository bookingRepository, ICartRepository cartRepository , UserManager<ApplicationUser> userManager,IBookingItemRepository bookingItemRepository)
        {
            this.bookingRepository = bookingRepository;
            this.cartRepository = cartRepository;
            this.userManager = userManager;
            this.bookingItemRepository = bookingItemRepository;
        }
        public IActionResult Success(int bookingId)
        {
            var currentUser = userManager.GetUserId(User);
            var itemsInCart = cartRepository.Get(e => e.ApplicationUserId == currentUser);
            var booking = bookingRepository.GetOne(e=>e.Id == bookingId);
            if (booking != null)
            {
                var service = new SessionService();
                var session = service.Get(booking.SessionId);

                booking.PaymentStripId = session.PaymentIntentId;
                booking.Status = true;
                booking.PaymentStatus = true;
                bookingRepository.Commit();

                // empty the cart after success payment method
                var ItemsToDelete = bookingItemRepository.Get(e=>e.BookingId == bookingId);
                foreach(var item in ItemsToDelete)
                {
                    var itemToDeleteInCart = cartRepository.GetOne(e => e.MovieId == item.MovieId && e.ApplicationUserId == currentUser);
                    cartRepository.Delete(itemToDeleteInCart);
                    cartRepository.Commit();
                }

               

            }

            // 1. empty cart.
            // 2.display bookings in booking page.
            // 3. refund.


         
            return View();
        }
        public IActionResult Cancel()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
