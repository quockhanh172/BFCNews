using System.ComponentModel.DataAnnotations;

namespace BFCNews.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
