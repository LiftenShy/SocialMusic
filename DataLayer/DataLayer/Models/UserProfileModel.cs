using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.DataLayer.Models
{
    public class UserProfileModel
    {
        public string LoginName { get; set; }

        public byte[] Password { get; set; }
    }
}
