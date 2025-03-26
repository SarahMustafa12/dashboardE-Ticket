using System.ComponentModel.DataAnnotations;

namespace E_TicketMovies.View_Models
{
    public class RegisterVM
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public List<string>? Roles { get; set; } = new List<string>();

    }
}
