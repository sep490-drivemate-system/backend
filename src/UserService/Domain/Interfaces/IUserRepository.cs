using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> IsEsxitEmail(string email);
        Task<bool> IsEsxitPhone(string phone);
        Task<bool> IsEsxitUserName(string userName);
    }
}
