using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace E_TicketMovies.Models
{
    
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "it must be more than 3 letters")]
        [MaxLength(250)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        [Required (ErrorMessage = "it is required")]
        public decimal Price { get; set; }

        public string? ImgUrl { get; set; }

        public string? TrailerUrl { get; set; }
        [Required(ErrorMessage = "it is required")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "it is required")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "it is required")]
        public int? MovieStatus { get; set; }
        [Required(ErrorMessage = "it is required")]
        public int? CinemaId { get; set; }
        [Required(ErrorMessage = "it is required")]
        public int? CategoryId { get; set; }
        [ValidateNever]
        public  Category Category { get; set; } = null!;
        [ValidateNever]
        public  Cinema Cinema { get; set; } = null!;
        [ValidateNever]
        public ICollection<ActorMovie> Actors { get; set; } = new List<ActorMovie>();
    }
}
