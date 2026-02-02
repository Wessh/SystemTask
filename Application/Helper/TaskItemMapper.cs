using Application.Dtos;
using Domain.Entities;

namespace Application.Helper
{
    public static class TaskItemMapper
    {
        public static TaskItemDto ToDto(TaskItem taskItem) =>
            new TaskItemDto(
                taskItem.Id,
                taskItem.Title!,
                taskItem.Description!,
                taskItem.Status,
                taskItem.DueDate
            );

        public static TaskItem ToEntity(CreateTaskItemDto dto) =>
            new TaskItem(dto.Title, dto.Description, dto.DueDate);
    }

}
