using DataLayer.Models.BaseModels;
using System.Collections.Generic;

namespace DataLayer.Models.AuthModels
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
