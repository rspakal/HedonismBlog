using System.ComponentModel.DataAnnotations;

namespace HedonismBlog.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string TimeStamp { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(500, ErrorMessage = "Comment must be between 2 and 500 characters long.", MinimumLength = 2)]
        public string Content { get; set; }
    }
}
