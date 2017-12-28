using SocialMusic.Models.EntityModels.AuthModels;

namespace SocialMusic.Buisness.Interfaces
{
    public interface ITokenService
    {
        void Authorize(UserProfile user);
    }
}
