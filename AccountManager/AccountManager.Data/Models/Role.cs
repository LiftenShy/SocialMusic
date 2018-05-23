using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManager.Data.Models
{
    public class Role
    {
        [Key]
        public long RoleId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        public virtual ICollection<AccountRole> Accounts { get; set; }
    }
}
