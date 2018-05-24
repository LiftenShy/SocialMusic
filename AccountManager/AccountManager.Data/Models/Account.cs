using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManager.Data.Models
{
    public class Account
    {
        [Key]
        public long AccountId { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [StringLength(50)]
        [Column(TypeName = "nvarchar(500)")]

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public User User { get; set; }

        public ICollection<AccountRole> Roles { get; set; }
    }
}
