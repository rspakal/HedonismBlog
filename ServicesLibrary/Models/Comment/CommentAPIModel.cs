namespace ServicesLibrary.Models.Comment
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Content { get; set; }
        public string TimeStamp { get; set; }
    }
}
