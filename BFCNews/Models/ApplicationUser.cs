using Microsoft.AspNetCore.Identity;

namespace BinhdienNews.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DayOfBirths { get; set; }
        public string Avatar { get; set; }
        public List<DepartmentUser> DepartmentUsers{ get; set; }

    }
}
