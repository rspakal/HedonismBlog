using System.ComponentModel.DataAnnotations;

namespace ServicesLibrary.Models.User
{ 
    public class UserLoginModel
    {

        /// <summary>
        /// User email.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email incorrect format")]
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "Password must be between 6 and 20 characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
