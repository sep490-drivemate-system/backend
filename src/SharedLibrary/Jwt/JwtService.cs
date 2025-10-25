using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.SharedKernel.Enum;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GenerateAccessToken(object obj)
        {
            var claims = ExtractClaimsFromObject(obj);
            return Task.FromResult(GenerateJwtToken(claims));
        }

        public Task<string> GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(randomBytes);
            return Task.FromResult(token);
        }



        private IEnumerable<Claim> ExtractClaimsFromObject(object obj)
        {
            var claims = new List<Claim>();

            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = prop.GetValue(obj);
                if (value == null) continue;

                string claimValue = prop.PropertyType.IsEnum
                    ? Enum.GetName(prop.PropertyType, value) ?? value.ToString()
                    : value.ToString();

                if (string.IsNullOrEmpty(claimValue)) continue;

                claims.Add(new Claim(prop.Name.ToLower(), claimValue));
            }

            var roleProp = obj.GetType().GetProperty("Role") ?? obj.GetType().GetProperty("role");
            if (roleProp != null)
            {
                var roleValue = roleProp.GetValue(obj)?.ToString();
                if (!string.IsNullOrEmpty(roleValue))
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleValue));
                }
            }

            return claims;
        }


        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(3)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       

        public Task<Guid> ExtractUserIdFromToken(string rawToken)
        {
            if (string.IsNullOrWhiteSpace(rawToken))
                return Task.FromResult(Guid.Empty);

            var token = rawToken.StartsWith("Bearer ") ? rawToken.Substring(7) : rawToken;

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id");

                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    return Task.FromResult(userId);
                }
            }
            catch
            {
            }

            return Task.FromResult(Guid.Empty);
        }

        public Task<UserRole> ExtractUserRoleFromToken(string rawToken)
        {
            if (string.IsNullOrWhiteSpace(rawToken))
                return Task.FromResult(UserRole.Demo);

            var token = rawToken.StartsWith("Bearer ") ? rawToken.Substring(7) : rawToken;

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Role");

                if (roleClaim != null && Enum.TryParse<UserRole>(roleClaim.Value, out var userRole))
                {
                    return Task.FromResult(userRole);
                }
            }
            catch
            {
            }

            return Task.FromResult(UserRole.Demo);
        }
    }
}
