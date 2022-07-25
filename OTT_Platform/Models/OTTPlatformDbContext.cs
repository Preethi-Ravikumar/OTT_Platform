using Microsoft.EntityFrameworkCore;

namespace OTT_Platform.Models
{
    public class OTTPlatformDbContext:DbContext
    {
        public OTTPlatformDbContext(DbContextOptions<OTTPlatformDbContext> options):base(options)
        {

        }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
