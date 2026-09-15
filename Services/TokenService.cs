using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Models;

namespace STUDENTPROJECTMANAEMENTSYSTEMBACKEND.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            // Claims representing the user identity and authorization details
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.FullName) ? user.Email : user.FullName)
            };

            // Add role claim from UserType if present
            if (!string.IsNullOrEmpty(user.UserType?.UserTypeName))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.UserType.UserTypeName));
            }

            // Add role claims from UserRoles if present
            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    if (userRole.Role != null && !string.IsNullOrEmpty(userRole.Role.RoleName) &&
                        !claims.Any(c => c.Type == ClaimTypes.Role && c.Value == userRole.Role.RoleName))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleName));
                    }
                }
            }

            // Add department claim for policy-based authorization
            if (!string.IsNullOrEmpty(user.Department))
            {
                claims.Add(new Claim("Department", user.Department));
            }

            // Locks the card with our secret key
            var keyString = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured in appsettings.json.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresInMinutes = double.TryParse(_config["Jwt:ExpiresInMinutes"], out var minutes) ? minutes : 60;

            // JwtSecurityToken: the actual token with issuer, audience, claims, and expiry
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
