using AccessControl.Buisness.Interfaces;
using AccessControl.Buisness.Settings;
using DataLayer.Models.AuthModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccessControl.Buisness.Implements
{
    public class JwtTokenService : ITokenService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public JwtTokenService(
            IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public SecurityToken Authorize(UserProfile user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.LoginName));
            //foreach (var role in user.UserRole.Roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role.Name));
            //}

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                )
            };
           return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
