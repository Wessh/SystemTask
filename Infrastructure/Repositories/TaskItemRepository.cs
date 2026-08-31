using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _dbContext;
        public TaskItemRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TaskItem task)
        {
            if(task is null)
                throw new ArgumentNullException(nameof(task));
            await _dbContext.TaskItems.AddAsync(task);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            var taskItem = await _dbContext.TaskItems.FindAsync(id);
            return taskItem;
        }

        public async Task<IEnumerable<TaskItem>> GetByStatusAsync(StatusTask status)
        {
            var tasksByStatus = await _dbContext.TaskItems
                .Where(taskItem => status == taskItem.Status)
                .AsNoTracking()
                .ToListAsync();

            return tasksByStatus;
        }

        public async Task UpdateAsync(TaskItem task)
        {
            if(task is null)
                throw new ArgumentNullException(nameof(task));

             _dbContext.Update(task);
            await _dbContext.SaveChangesAsync();
        }
    }
}
