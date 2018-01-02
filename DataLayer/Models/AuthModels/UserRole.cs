using DataLayer.Models.BaseModels;

namespace DataLayer.Models.AuthModels
{
    public class UserRole : BaseEntity
    {
        public int RoleId { get; set; }

        public int UserId { get; set; }

        public virtual UserProfile UserProfiles { get; set;}

        public virtual Role Roles { get; set; }
    }
}
