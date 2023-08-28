using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BFCNews.ModelsView
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username không được để trống.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password không được để trống.")]
        public string Password { get; set; }
    }
}
