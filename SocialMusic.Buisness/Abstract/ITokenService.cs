using Microsoft.IdentityModel.Tokens;
using SocialMusic.Models.EntityModels.AuthModels;

namespace SocialMusic.Buisness.Interfaces
{
    public interface ITokenService
    {
        SecurityToken Authorize(UserProfile user);
    }
}
