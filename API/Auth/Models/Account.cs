using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Models
{
    public class Account
    {
        [Key]
        public long AccountId { get; set; }

        [ForeignKey("UserId")]
        public long UserId { get; set; }

        public string NickName { get; set; }
    }
}
