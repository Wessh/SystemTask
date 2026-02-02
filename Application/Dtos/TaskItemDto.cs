using Domain.Enums;

namespace Application.Dtos;
public record TaskItemDto(
    Guid Id,
    string Title,
    string Description,
    StatusTask Status,
    DateTime DueDate);