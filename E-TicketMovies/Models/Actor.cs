using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace E_TicketMovies.Models
{

    public class Actor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The is required")]
        [MinLength(3, ErrorMessage = "it must be more than 3 letters")]
        [MaxLength(60)]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "The is required")]
        [MinLength(3, ErrorMessage = "it must be more than 3 letters")]
        [MaxLength(60)]
        public string LastName { get; set; } = null!;
        
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? News { get; set; }
        [ValidateNever]
        public  ICollection<ActorMovie> Movies { get; set; } = new List<ActorMovie>();
    }
}
