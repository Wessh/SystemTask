using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface ITaskItemRepository
    {
        public Task<IEnumerable<TaskItem>> GetByStatusAsync(StatusTask status);
        public Task<TaskItem?> GetByIdAsync(Guid id);
        public Task AddAsync(TaskItem task);
        public Task UpdateAsync(TaskItem task);
    }
}
