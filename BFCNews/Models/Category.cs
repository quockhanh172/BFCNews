namespace BFCNews.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public List<Post> Posts { get; set; }
    }
}
