using AccountManager.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Data
{
    public class AccountManagerContext : DbContext
    {
        public AccountManagerContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.Account)
                .WithOne(u => u.User)
                .HasForeignKey<Account>(acc => acc.AccountId);

            modelBuilder.Entity<Account>()
                .HasMany(acc => acc.Roles)
                .WithOne(accr => accr.Account);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Accounts)
                .WithOne(accr => accr.Role);
        }
    }
}
