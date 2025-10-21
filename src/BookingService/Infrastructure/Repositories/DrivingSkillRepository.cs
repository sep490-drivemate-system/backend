using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repositories
{
    public class DrivingSkillRepository(BookingDbContext context): IDrivingSkillRepository
    {
        private readonly BookingDbContext _context = context;

        public async Task<DrivingSkill> CreateDrivingSkill(DrivingSkill info)
        {
            await _context.DrivingSkills.AddAsync(info);
            return info;
        }

        public async Task<List<DrivingSkill>> GetAllDrivingSkill()
        {
            return await _context.DrivingSkills.ToListAsync();
        }

        public async Task<DrivingSkill?> GetDrivingSkillById(Guid id)
        {
            return await _context.DrivingSkills.FindAsync(id);
        }

        public async Task<DrivingSkill> RemoveDrivingSkill(Guid id)
        {
            var item = await _context.DrivingSkills.FindAsync(id);

            if (item == null)
            {
                throw new Exception("Can not find the driving skill with the given id");
            }

            item.IsDeleted = true;

            _context.DrivingSkills.Remove(item);

            return item;
        }

        public Task<DrivingSkill> UpdateDrivingSkill(DrivingSkill info)
        {
            throw new NotImplementedException();
        }
    }
}
