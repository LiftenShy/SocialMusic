using AccountManager.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Data
{
    public class AccountManagerDbContext : IdentityDbContext<ApplicationUser>
    {
        public AccountManagerDbContext(DbContextOptions<AccountManagerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
