using System.ComponentModel.DataAnnotations;

namespace authAPI.Model
{
    public class UserLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
