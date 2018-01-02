using DataLayer.Models.AuthModels;
using Microsoft.IdentityModel.Tokens;

namespace AccessControl.Buisness.Interfaces
{
    public interface ITokenService
    {
        SecurityToken Authorize(UserProfile user);
    }
}
