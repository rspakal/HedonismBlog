﻿using System.Collections.Generic;

namespace ServicesLibrary.Models.Post
{
    public class PostUpdateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserEmail { get; set; }
        public List<TagModel> Tags { get; set; }
    }
}