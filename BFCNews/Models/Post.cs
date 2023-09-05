using BinhdienNews.Models;

namespace BFCNews.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public List<Comment> Comments { get; set; }
        public int ViewsCount { get; set; }
        public int LikesCount { get; set; }
        public Boolean Status { get; set; }
        public ApplicationUser User { get; set; }
        public Department Department { get; set; }
        public Category Category { get; set; }
        public string File { get; set; }
        public List<ImageOfPost> ImageOfPosts { get; set; }
    }
}
