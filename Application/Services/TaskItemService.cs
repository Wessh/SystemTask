using Application.Dtos;
using Application.Helper;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _repository;
        public TaskItemService(ITaskItemRepository repository) 
        {
            _repository = repository;
        }

        public async Task<TaskItemDto> AddAsync(CreateTaskItemDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Título da tarefa não pode ser vazio.");

            if (dto.DueDate < DateTime.UtcNow)
                throw new ArgumentException("Data de vencimento deve ser futura.");

            var taskItem = TaskItemMapper.ToEntity(dto);;
            await _repository.AddAsync(taskItem);

            return TaskItemMapper.ToDto(taskItem);
        }

        public async Task<TaskItemDto> GetByIdAsync(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException("Id da tarefa não pode ser vazio.", nameof(id));

            var taskItem = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Tarefa com id {id} não encontrada.");

            return TaskItemMapper.ToDto(taskItem);
        }

        public async Task<IEnumerable<TaskItemDto>> GetByStatusAsync(StatusTask status)
        {
            if (!Enum.IsDefined(typeof(StatusTask), status)) 
                throw new ArgumentException("Status inválido.", nameof(status));

            var taskItems = await _repository.GetByStatusAsync(status);

            var taskItemDtos = taskItems.Select(TaskItemMapper.ToDto).ToList(); // Uso de method group
            
            return taskItemDtos;
        }

        public async Task<TaskItemDto> StartAsync(Guid id)
        {
            if(id == Guid.Empty)
                throw new ArgumentException("Id da tarefa não pode ser vazio.", nameof(id));

            var taskItem = await _repository.GetByIdAsync(id);
            if (taskItem == null)
                throw new KeyNotFoundException($"Tarefa com id {id} não encontrada.");

            taskItem.StartTask();
            await _repository.UpdateAsync(taskItem);
            
            return TaskItemMapper.ToDto(taskItem);
        }

        public async Task<TaskItemDto> OnHoldAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id da tarefa não pode ser vazio.", nameof(id));

            var taskItem = await _repository.GetByIdAsync(id);
            if (taskItem == null)
                throw new KeyNotFoundException($"Tarefa com id {id} não encontrada.");

            taskItem.OnHoldTask();
            await _repository.UpdateAsync(taskItem);

            return TaskItemMapper.ToDto(taskItem);
        }

        public async Task<TaskItemDto> CompleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id da tarefa não pode ser vazio.", nameof(id));

            var taskItem = await _repository.GetByIdAsync(id);
            if (taskItem == null)
                throw new KeyNotFoundException($"Tarefa com id {id} não encontrada.");

            taskItem.CompleteTask();
            await _repository.UpdateAsync(taskItem);

            return TaskItemMapper.ToDto(taskItem);
        }
        
        public async Task<TaskItemDto> CancelAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id da tarefa não pode ser vazio.", nameof(id));

            var taskItem = await _repository.GetByIdAsync(id);
            if (taskItem == null)
                throw new KeyNotFoundException($"Tarefa com id {id} não encontrada.");

            taskItem.CancelTask();
            await _repository.UpdateAsync(taskItem);

            return TaskItemMapper.ToDto(taskItem);
        }
    }
}
