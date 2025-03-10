namespace E_TicketMovies.Models
{

    public class Actor
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; }

        public string? News { get; set; }

        public  ICollection<ActorMovie> Movies { get; set; } = new List<ActorMovie>();
    }
}
