using System.Security.Claims;
using SharedLibrary.SharedKernel.Enum;

namespace SharedLibrary.SharedKernel.Identity
{
    public interface IUserClaimsAccessor
    {
      Guid GetUserId(ClaimsPrincipal? user);
      UserRole GetUserRole(ClaimsPrincipal? user);
    }
}

