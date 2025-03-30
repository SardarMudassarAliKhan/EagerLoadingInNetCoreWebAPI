using EagerLoadingInNetCoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EagerLoadingInNetCoreWebAPI
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().HasData(new Blog { BlogId = 1, Title = "Tech with Jane" });

            modelBuilder.Entity<Post>().HasData(
                new Post { PostId = 1, Content = "ASP.NET Core is awesome!", BlogId = 1 },
                new Post { PostId = 2, Content = "Entity Framework Tips", BlogId = 1 }
            );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { CommentId = 1, Message = "Loved it!", AuthorName = "John", PostId = 1 },
                new Comment { CommentId = 2, Message = "Very helpful!", AuthorName = "Alice", PostId = 1 },
                new Comment { CommentId = 3, Message = "Great explanation.", AuthorName = "Sara", PostId = 2 }
            );
        }
    }
}
