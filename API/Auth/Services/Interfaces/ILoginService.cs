using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Auth.Services.Interfaces
{
    public interface ILoginService
    {
        Task<object> GenerateJwtToken(string email, IdentityUser user);
    }
}
