using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence.Context;

namespace UserService.Infrastructure.Repositories
{
    public class InstructorRepository(UserServiceDbContext context): GenericRepository<Instructor>(context), IInstructorRepository
    {

    }
}
