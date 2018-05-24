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
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(a => a.Account)
                .WithOne(u => u.User)
                .HasForeignKey<Account>(acc => acc.AccountId);

            modelBuilder.Entity<AccountRole>()
                .HasKey(ar => new {ar.AccountId, ar.RoleId});

            modelBuilder.Entity<AccountRole>()
                .HasOne(ar => ar.Account)
                .WithMany(a => a.Roles)
                .HasForeignKey(ar => ar.AccountId);

            modelBuilder.Entity<AccountRole>()
                .HasOne(ar => ar.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(ar => ar.RoleId);
        }
    }
}
