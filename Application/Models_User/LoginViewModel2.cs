using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application
{
    public class LoginViewModel2
    {

        // Existing properties
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        // New property for profile photo upload
        [Display(Name = "Profile Photo")]
        public IFormFile ProfilePhoto { get; set; }
    }
}
