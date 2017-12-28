using Microsoft.EntityFrameworkCore;
using SocialMusic.Models.EntityModels;
using SocialMusic.Models.EntityModels.AuthModels;

namespace SocialMusic.Data
{
    public class SocialMusicContext : DbContext
    {
        public SocialMusicContext(DbContextOptions<SocialMusicContext> options)
            : base(options)
        { }

        public DbSet<Person> Persons { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Roles)
                .WithMany(r => r.UserRole);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.UserProfiles)
                .WithMany(up => up.UserRole);
        }
    }
}
