using E_TicketMovies.Models;
using E_TicketMovies.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_TicketMovies.Areas.End_User.Controllers
{
    [Area("End User")]
    public class BaseController : Controller
    {
        private readonly ICartRepository cartRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public BaseController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager)
        {
            this.cartRepository = cartRepository;
            this.userManager = userManager;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var currentUser = userManager.GetUserId(User);
            var cartItemsNum = cartRepository.Get(e=>e.ApplicationUserId == currentUser).Sum(e =>e.Count);
           
            ViewBag.CartItemsNum = cartItemsNum;
            base.OnActionExecuted(context);
        }
    }
}
