using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
