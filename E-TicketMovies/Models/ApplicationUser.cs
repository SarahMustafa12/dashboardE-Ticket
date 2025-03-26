using Microsoft.AspNetCore.Identity;

namespace E_TicketMovies.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string?  Address { get; set; }
    }
}
