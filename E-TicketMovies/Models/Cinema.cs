using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace E_TicketMovies.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3,  ErrorMessage = "it must be more than 3 letters")]
        [MaxLength(250)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        [RegularExpression("^.*\\.(png|jpg|jpeg|gif|bmp|webp|tiff|tif|svg)$", ErrorMessage = "Only image files are allowed.")]

        public string? CinemaLogo { get; set; }

        public string? Address { get; set; }
        [ValidateNever]
        public  ICollection<Movie> Movies { get; set; } = new List<Movie>();

    }
}
