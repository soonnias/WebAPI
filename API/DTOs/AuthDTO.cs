using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public class LoginUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
