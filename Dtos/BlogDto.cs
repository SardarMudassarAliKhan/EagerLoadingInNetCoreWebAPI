namespace EagerLoadingInNetCoreWebAPI.Dtos
{
    public class BlogDto
    {
        public string Title { get; set; }
        public List<PostDto> Posts { get; set; }
    }
}
