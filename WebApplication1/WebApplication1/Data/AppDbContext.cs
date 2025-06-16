using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<MetadataImage> MetadataImages => Set<MetadataImage>();
        public DbSet<Request> requests => Set<Request>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<MetadataImage>().HasKey(m => m.MetaImage_ID);
            modelBuilder.Entity<Request>().HasKey(r => r.Id);

            modelBuilder.Entity<Request>().HasOne(r => r.User)
                                          .WithMany()
                                          .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Request>().HasOne(r => r.MetaImage)
                                          .WithMany()
                                          .HasForeignKey(r => r.MetaImage_Id);
        }

    }
}
