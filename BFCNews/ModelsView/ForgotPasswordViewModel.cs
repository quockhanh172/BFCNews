using System.ComponentModel.DataAnnotations;

namespace BFCNews.ModelsView
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
