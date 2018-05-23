using System.ComponentModel.DataAnnotations;

namespace AccountManager.Data.Models
{
    public class AccountRole
    {
        [Key]
        public long AccountRoleId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
    }
}
