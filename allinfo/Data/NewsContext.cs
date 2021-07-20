using Microsoft.EntityFrameworkCore;
using allinfo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace allinfo.Data
{
    public class NewsContext : IdentityDbContext<Writer, IdentityRole<int>, int>
    {
        public NewsContext (DbContextOptions<NewsContext> options)
            : base(options)
        {}

        public DbSet<Article> Articles { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Comment> Comments { get; set;}
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Prospect> Prospects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>().ToTable("Articles");
            modelBuilder.Entity<Reader>().ToTable("Readers");
            modelBuilder.Entity<Writer>().ToTable("Writers");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Player>().ToTable("Players");
            modelBuilder.Entity<Nationality>().ToTable("Nationalities");
            modelBuilder.Entity<Prospect>().ToTable("Prospects");
        }
    }
}