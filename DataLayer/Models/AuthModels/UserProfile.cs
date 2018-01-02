using DataLayer.Models.BaseModels;
using System.Collections.Generic;

namespace DataLayer.Models.AuthModels
{
    public class UserProfile : BaseEntity
    {
        public string LoginName { get; set; }

        public byte[] PasswordHash { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
