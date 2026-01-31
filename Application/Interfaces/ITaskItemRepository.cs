using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface ITaskItemRepository
    {
        /// <summary> 
        /// Obtém todas as tarefas filtradas por um determinado status. 
        /// </summary> 
        /// <param name="status">Status da tarefa (Pending, InProgress, Completed, Cancelled, OnHold).</param> 
        /// <returns>Lista de tarefas que possuem o status informado.</returns>
        public Task<IEnumerable<TaskItem>> GetByStatusAsync(StatusTask status);
        /// <summary>
        /// Obtém uma tarefa pelo identificador único
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>Tarefa com o identificador único informado</returns>
        public Task<TaskItem?> GetByIdAsync(Guid id);
        /// <summary>
        /// Adiciona um tarefa a base de dados.
        /// </summary>
        /// <param name="task">Tarefa</param>
        public Task AddAsync(TaskItem task);
        /// <summary>
        /// Atualiza a tarefa desejada.
        /// </summary>
        /// <param name="task">Tarefa</param>
        public Task UpdateAsync(TaskItem task);
    }
}
