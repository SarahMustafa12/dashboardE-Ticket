namespace E_TicketMovies.Models
{
    
    public class Movie
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? ImgUrl { get; set; }

        public string? TrailerUrl { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MovieStatus { get; set; }

        public int CinemaId { get; set; }

        public int CategoryId { get; set; }

        public  Category Category { get; set; } = null!;

        public  Cinema Cinema { get; set; } = null!;

        public ICollection<ActorMovie> Actors { get; set; } = new List<ActorMovie>();
    }
}
