using SocialMusic.Models.EntityModels.BaseModels;
using System.Collections.Generic;

namespace SocialMusic.Models.EntityModels.AuthModels
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
