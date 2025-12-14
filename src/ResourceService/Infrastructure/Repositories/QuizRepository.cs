using Microsoft.EntityFrameworkCore;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Repositories;
using ResourceService.Infrastructure.Commons;
using ResourceService.Infrastructure.Persistences;

namespace ResourceService.Infrastructure.Repositories
{
    public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
    {
        public QuizRepository(ResourceDbContext context) : base(context) { }
    }
}


