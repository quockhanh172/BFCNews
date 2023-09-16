using BinhdienNews.Models;

namespace BFCNews.Models
{
    public class Post
    {
        public Post()
        {
            Files = new List<FileOfPost>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public List<Comment> Comments { get; set; }
        public Boolean Status { get; set; }
        public ApplicationUser User { get; set; }
        public Department Department { get; set; }
        public Category Category { get; set; }
        public List<FileOfPost> Files { get; set; }
    }
}
