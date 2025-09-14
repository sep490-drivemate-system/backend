using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(rf => rf.Id == refreshTokenId);
        }

    }
}
