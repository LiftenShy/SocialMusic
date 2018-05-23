
using System;
using System.ComponentModel.DataAnnotations;

namespace AccountManager.Data.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
        public string FirstName { get; set; }
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Bithrday { get; set; }
        public int? Age { get; set; }

        public virtual Account Account { get; set; }
    }
}
