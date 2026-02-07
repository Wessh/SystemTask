using Application.Dtos;
using Domain.Entities;

namespace Application.Helper
{
    public static class TaskItemMapper
    {
        public static TaskItemDto ToDto(TaskItem taskItem) =>
            new TaskItemDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title!,
                Description = taskItem.Description!,
                Status = taskItem.Status,
                DueDate = taskItem.DueDate
            };

        public static TaskItem ToEntity(CreateTaskItemDto dto) =>
            new TaskItem(dto.Title, dto.Description, dto.DueDate);
    }

}
