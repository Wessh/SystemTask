using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public record CreateTaskItemDto
{
    [Required(ErrorMessage = "Título é obrigatório")]
    public required string Title { get; init; }
    [Required(ErrorMessage = "Descrição é obrigatória")]
    public required string Description { get; init; }
    [Required(ErrorMessage = "Data de vencimento é obrigatória")]
    public DateTime DueDate { get; init; }
}
    /*
    (
    string Title,
    string Description,
    DateTime DueDate);
    */
