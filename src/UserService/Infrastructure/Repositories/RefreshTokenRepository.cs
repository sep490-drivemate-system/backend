using Microsoft.AspNetCore.Identity.Data;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshRequest>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context) { }

    }
}
