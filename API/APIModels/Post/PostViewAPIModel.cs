using API.APIModels.Comment;
using BlogDALLibrary.Entities;
using System.Collections.Generic;

namespace API.APIModels.Post
{
    public class PostViewAPIModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserEmail { get; set; }
        public List<CommentAPIModel> Comments { get; set; } = new();
        public List<TagAPIModel> Tags { get; set; } = new();

    }
}
