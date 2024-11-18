using HedonismBlog.ViewModels.Validation;
using System.ComponentModel.DataAnnotations;

namespace HedonismBlog.ViewModels
{
    public class TagViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(10, ErrorMessage = "Tag name must be between 3 and 10 characters long.", MinimumLength = 3)]
        [LatinCharactersOnly(ErrorMessage ="Only latin symbols allowed")]
        public string Text { get; set; }
        public bool IsSelected { get; set; }
    }
}
