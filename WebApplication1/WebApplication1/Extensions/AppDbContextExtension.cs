using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Extensions
{
    public static class AppDbContextExtensions
    {
        public static async Task SaveMetadataAsync(this AppDbContext db, MetadataImage metadata)
        {
            db.MetadataImages.Add(metadata);
            await db.SaveChangesAsync();
        }
    }
}
