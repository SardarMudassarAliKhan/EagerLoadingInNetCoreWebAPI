namespace EagerLoadingInNetCoreWebAPI.Dtos
{
    public class PostDto
    {
        public string Content { get; set; }
        public List<CommentDto> Comments { get; set; }
    }

}
