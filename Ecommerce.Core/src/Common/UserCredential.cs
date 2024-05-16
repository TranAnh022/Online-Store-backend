using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.src.Common
{
    public class UserCredential
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }


    }
}