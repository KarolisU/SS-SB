using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SSSB.Auth.Model;
using SSSB.Data.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SSSB.Auth
{
    public interface ITokenManager
    {
        Task<string> CreateAccessTokenAsync(SSSBUser user);
    }

    public class TokenManager : ITokenManager
    {
        private readonly UserManager<SSSBUser> _userManager;
        private readonly SymmetricSecurityKey _authSigningKey;
        private readonly string _issuer;
        private readonly string _audience;
        public TokenManager(IConfiguration configuration, UserManager<SSSBUser> userManager)
        {
            _userManager = userManager;
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _issuer = configuration["JWT:ValidIssuer"];
            _audience = configuration["JWT:ValidAudience"];
        }

        public async Task<string> CreateAccessTokenAsync(SSSBUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(CustomClaims.UserId, user.Id),
            };
            authClaims.AddRange(userRoles.Select(userRoles => new Claim(ClaimTypes.Role, userRoles)));

            var accessSecurityToker = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                expires: DateTime.UtcNow.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(accessSecurityToker);
        }
    }
}
