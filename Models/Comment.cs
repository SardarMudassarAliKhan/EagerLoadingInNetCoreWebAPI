namespace EagerLoadingInNetCoreWebAPI.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Message { get; set; }
        public string AuthorName { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
