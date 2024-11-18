using HedonismBlog.Models;
using HedonismBlog.ViewModels.Validation;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HedonismBlog.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required1.")]
        [StringLength(50, ErrorMessage = "Title must be between 5 and 50 characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required1.")]
        [StringLength(1000, ErrorMessage = "Content must be between 10 and 1000 characters long.", MinimumLength = 10)]
        public string Content { get; set; }
        public string UserEmail { get; set; }
        public CommentViewModel NewComment { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}
