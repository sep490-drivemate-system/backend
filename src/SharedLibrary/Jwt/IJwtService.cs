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
    public interface IJwtService
    {
        public Task<string> GenerateAccessToken(object obj);

        public Task<string> GenerateRefreshToken();

        public Task<Guid> ExtractUserIdFromToken(string rawToken);
        public Task<UserRole> ExtractUserRoleFromToken(string rawToken);
    }
}
