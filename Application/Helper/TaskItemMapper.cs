using Application.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    internal static class TaskItemMapper
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
