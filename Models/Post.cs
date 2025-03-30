using System.Xml.Linq;

namespace EagerLoadingInNetCoreWebAPI.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
