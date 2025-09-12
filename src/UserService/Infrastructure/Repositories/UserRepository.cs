using Microsoft.EntityFrameworkCore;
using System.Numerics;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }
        public async Task<bool> IsEsxitEmail(string email)=> await _context.Users.AnyAsync(u => u.Email == email);

        public async Task<bool> IsEsxitPhone(string phone) => await _context.Users.AnyAsync(u => u.PhoneNumber == phone);

        public async Task<User?> IsExistUser(string emailOrPhone)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == emailOrPhone || u.PhoneNumber == emailOrPhone);
        }

        public async Task<bool> IsEsxitUserName(string userName) => await _context.Users.AnyAsync(u => u.UserName == userName);

    }
}
