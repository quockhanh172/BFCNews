namespace BinhdienNews.Models
{
    public class Position
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public bool Status { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}