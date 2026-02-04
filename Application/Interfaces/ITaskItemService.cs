using Application.Dtos;
using Domain.Enums;

namespace Application.Interfaces;
public interface ITaskItemService
{
    /// <summary>
    /// Adiciona uma nova tarefa no sistema.
    /// </summary>
    /// <param name="dto">Objeto contendo título, descrição e prazo da tarefa.</param>
    /// <returns>Retorna o DTO da tarefa criada, incluindo ID e status inicial.</returns>
    public Task<TaskItemDto> AddAsync(CreateTaskItemDto dto);
    /// <summary>
    /// Obtém uma tarefa pelo id informado.
    /// </summary>
    /// <param name="id">Identificador da tarefa.</param>
    /// <returns>Retorna um DTO de tarefa ou null se não encontrar nada.</returns>
    public Task<TaskItemDto?> GetByIdAsync(Guid id);
    /// <summary>
    /// Obtém tarefas pelo Status informado.
    /// </summary>
    /// <param name="status">Status da tarefa (InProgress, Completed, etc.).</param>
    /// <returns>Lista de DTOs das tarefas encontradas ou uma lista vazia.</returns>
    public Task<IEnumerable<TaskItemDto>> GetByStatusAsync(StatusTask status);

    /// <summary>
    /// Inicia a execução de uma tarefa.
    /// </summary>
    /// <param name="id">Identificador da tarefa.</param>
    /// <returns>Retorna o DTO atualizado da tarefa.</returns>
    public Task<TaskItemDto> StartAsync(Guid id);
    /// <summary>
    /// Coloca a tarefa em espera.
    /// </summary>
    /// <param name="id">Identificador da tarefa.</param>
    /// <returns>Retorna o DTO atualizado da tarefa.</returns>
    public Task<TaskItemDto> OnHoldAsync(Guid id);
    /// <summary>
    /// Marca a tarefa como concluida.
    /// </summary>
    /// <param name="id">Identificador da tarefa.</param>
    /// <returns>Retorna o DTO atualizado da tarefa.</returns>
    public Task<TaskItemDto> CompleteAsync(Guid id);
    /// <summary>
    /// Cancela a tarefa.
    /// </summary>
    /// <param name="id">Identificador da tarefa.</param>
    /// <returns>Retorna o DTO atualizado da tarefa.</returns>
    public Task<TaskItemDto> CancelAsync(Guid id);
}

