using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MongoDBSample.Domain.Model.Users
{
    public class User
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [PasswordPropertyText(true)]
        public string Password { get; set; } = string.Empty;
    }
}