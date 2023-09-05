namespace BFCNews.Models
{
    public class ImageOfPost
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public Post Post { get; set; }
    }
}
