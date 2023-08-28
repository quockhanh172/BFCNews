using System.ComponentModel.DataAnnotations;
namespace BFCNews.ModelsView
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string UserId { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Mật khẩu không trùng khớp.")]
        public string ConfirmPassword { get; set; }
        
    }
}
