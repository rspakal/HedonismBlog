using System.Collections.Generic;

namespace API.APIModels.Post
{
    public class PostCreateAPIModel
    {
        public string Tite { get; set; }
        public string Content { get; set; }
        public string UserEmail { get; set; }
        public List<TagAPIModel> Tags { get; set; }
    }
}
