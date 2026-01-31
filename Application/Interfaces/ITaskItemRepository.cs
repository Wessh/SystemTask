using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITaskItemRepository
    {
        public Task<IEnumerable<TaskItem>> GetByStatusAsync(string status);
        public Task<TaskItem?> GetByIdAsync(Guid id);
        public Task AddAsync(TaskItem task);
        public Task UpdateAsync(TaskItem task);
    }
}
