using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Enum;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class PolicyRepository(ApplicationDbContext context): GenericRepository<Policy>(context), IPolicyRepository
    {
        public async Task<List<Policy>> GetByPolicyTypeAsync(PolicyType policyType)
        {
            return await _context.Set<Policy>()
                .Where(p => p.PolicyType == policyType && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
