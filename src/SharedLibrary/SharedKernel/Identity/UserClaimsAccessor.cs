using SharedLibrary.SharedKernel.Enum;
using System.Security.Claims;

namespace SharedLibrary.SharedKernel.Identity
{
    public class UserClaimsAccessor : IUserClaimsAccessor
    {
        public Guid GetUserId(ClaimsPrincipal? user)
        {
            var idClaim = user.Claims.FirstOrDefault(c => c.Type == "id");
            return idClaim != null && Guid.TryParse(idClaim.Value, out var userId)
                ? userId
                : Guid.Empty;
        }

        public UserRole GetUserRole(ClaimsPrincipal? user)
        {
            var roleClaim = user.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Role || c.Type == "role");

            return roleClaim != null && System.Enum.TryParse<UserRole>(roleClaim.Value, out var role)
                ? role
                : UserRole.Demo;
        }
    }
}

