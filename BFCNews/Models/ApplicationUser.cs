using BFCNews.Models;
using Microsoft.AspNetCore.Identity;

namespace BinhdienNews.Models
{
    public class ApplicationUser : IdentityUser
    {
            public ApplicationUser()
        {
            DepartmentUsers = new List<DepartmentUser>(); // Khởi tạo DepartmentUsers trong constructor
        }
        public string FullName { get; set; }
        public DateTime DayOfBirths { get; set; }
        public string Avatar { get; set; }
        public List<DepartmentUser> DepartmentUsers{ get; set; }
        public List<Post> Posts{ get; set; }
    }
}
