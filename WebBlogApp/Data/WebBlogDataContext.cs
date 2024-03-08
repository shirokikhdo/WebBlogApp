using Microsoft.EntityFrameworkCore;

namespace WebBlogApp.Data
{
    public class WebBlogDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }

        public WebBlogDataContext(DbContextOptions contextOptions) : base(contextOptions)
        {
            Database.EnsureCreated();
        }
    }
}