using SocialMusic.Models.EntityModels.BaseModels;
using System.Collections.Generic;

namespace SocialMusic.Models.EntityModels.AuthModels
{
    public class UserProfile : BaseEntity
    {
        public string LoginName { get; set; }

        public byte[] PasswordHash { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
