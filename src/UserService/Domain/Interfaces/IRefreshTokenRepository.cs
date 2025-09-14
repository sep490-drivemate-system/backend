using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId);
    }
}
