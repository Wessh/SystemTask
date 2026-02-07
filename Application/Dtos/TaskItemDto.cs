using Domain.Enums;

namespace Application.Dtos;

public record TaskItemDto
 {
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public StatusTask Status { get; init; }
    public DateTime DueDate { get; init; }
  }
/*(
    Guid Id,
    string Title,
    string Description,
    StatusTask Status,
    DateTime DueDate
);*/

 