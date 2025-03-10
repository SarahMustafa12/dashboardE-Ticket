using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace E_TicketMovies.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3 , ErrorMessage = "it must be more than 3 letters")]
        [MaxLength(60)]
        public string Name { get; set; } = null!;
        [ValidateNever]
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
