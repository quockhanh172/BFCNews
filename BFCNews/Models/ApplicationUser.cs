using Microsoft.AspNetCore.Identity;

namespace BinhdienNews.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public Position Id_Position { get; set; }
        public Department Id_Department  { get; set; }
    }
}
