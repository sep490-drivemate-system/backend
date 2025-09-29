using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> DeleleRefreshToken(string refreshToken)
        {
            var refreshKey = await _context.RefreshTokens
                                           .FirstOrDefaultAsync(u => u.RefreshKey == refreshToken);

            if (refreshKey == null)
            {
                return false;
            }
            _context.RefreshTokens.Remove(refreshKey);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rf => rf.Id == refreshTokenId);
        }

    }
}
