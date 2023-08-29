namespace BFCNews.Models
{
    public class Tag
    {
        public int ID { get; set; }
        public string TagName { get; set; }
        public List<Post> Posts { get; set; }
        public bool Status { get; set; }
    }
}