using EagerLoadingInNetCoreWebAPI.Dtos;
using EagerLoadingInNetCoreWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EagerLoadingInNetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDto>>> GetAllBlogs()
        {
            var blogs = await _context.Blogs
                .Include(b => b.Posts)
                    .ThenInclude(p => p.Comments)
                .Select(b => new BlogDto
                {
                    Title = b.Title,
                    Posts = b.Posts.Select(p => new PostDto
                    {
                        Content = p.Content,
                        Comments = p.Comments.Select(c => new CommentDto
                        {
                            Message = c.Message,
                            AuthorName = c.AuthorName
                        }).ToList()
                    }).ToList()
                }).ToListAsync();

            return Ok(blogs);
        }

        // GET: api/blogs/1
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDto>> GetBlogById(int id)
        {
            var blog = await _context.Blogs
                .Where(b => b.BlogId == id)
                .Include(b => b.Posts)
                    .ThenInclude(p => p.Comments)
                .Select(b => new BlogDto
                {
                    Title = b.Title,
                    Posts = b.Posts.Select(p => new PostDto
                    {
                        Content = p.Content,
                        Comments = p.Comments.Select(c => new CommentDto
                        {
                            Message = c.Message,
                            AuthorName = c.AuthorName
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (blog == null) return NotFound();
            return Ok(blog);
        }

        // POST: api/blogs
        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog([FromBody] Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlogById), new { id = blog.BlogId }, blog);
        }

        // PUT: api/blogs/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] Blog updatedBlog)
        {
            if (id != updatedBlog.BlogId)
                return BadRequest("Blog ID mismatch");

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();

            blog.Title = updatedBlog.Title;
            _context.Entry(blog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Blogs.Any(e => e.BlogId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/blogs/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs
                .Include(b => b.Posts)
                    .ThenInclude(p => p.Comments)
                .FirstOrDefaultAsync(b => b.BlogId == id);

            if (blog == null) return NotFound();

            _context.Comments.RemoveRange(blog.Posts.SelectMany(p => p.Comments));
            _context.Posts.RemoveRange(blog.Posts);
            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
