﻿namespace E_TicketMovies.Models
{
    public class ActorMovie
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }

        
        public  Actor Actor { get; set; } = null!;
        public Movie Movie { get; set; } = null!;

    }
}
