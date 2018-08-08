using System.ComponentModel.DataAnnotations;

namespace Auth.ModelsDto
{
    public class AccountDto
    {
        public long AccountId { get; set; }

        public long UserId { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }
    }
}
