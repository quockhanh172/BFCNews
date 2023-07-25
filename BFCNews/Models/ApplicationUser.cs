using Microsoft.AspNetCore.Identity;

namespace BinhdienNews.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DayOfBirths { get; set; }
        public Position Position { get; set; }
        public Department Department  { get; set; }
    }
}
