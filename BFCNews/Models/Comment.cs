using BinhdienNews.Models;
using System.Security.Principal;

namespace BFCNews.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public DateTime CommentDate { get; set; }

        public Post Post { get; set; }

        public ApplicationUser User { get; set; }
        public bool Status { get; set; }
    }
}