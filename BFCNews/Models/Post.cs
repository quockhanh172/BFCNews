namespace BFCNews.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public virtual Category Category { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Tag> Tags { get; set; }
        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public Boolean Status { get; set; }

    }
}
