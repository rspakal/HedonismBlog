namespace API.APIModels.Comment
{
    public class CommentAPIModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserEmail { get; set; }
        public string Content { get; set; }
        public string TimeStamp { get; set; }
    }
}
