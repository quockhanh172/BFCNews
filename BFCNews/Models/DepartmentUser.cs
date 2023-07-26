namespace BinhdienNews.Models
{
    public class DepartmentUser
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public bool Status { get; set; }
        public ApplicationUser User { get; set; }
        public Department Department { get; set; }
    }
}