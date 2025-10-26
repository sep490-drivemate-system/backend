using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<bool> CreateRefreshToken(string refreshToken);
        Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId);
        Task<bool> DeleleRefreshToken(string refreshToken);
    }
}
