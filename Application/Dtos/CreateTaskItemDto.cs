namespace Application.Dtos;

public record CreateTaskItemDto(
    string Title,
    string Description,
    DateTime DueDate);

