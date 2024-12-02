using ServicesLibrary.Models.Comment;
using System.Collections.Generic;

namespace ServicesLibrary.Models.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserEmail { get; set; }
        public CommentModel NewComment { get; set; }
        public List<CommentModel> Comments { get; set; } = new();
        public List<TagModel> Tags { get; set; } = new();

    }
}
