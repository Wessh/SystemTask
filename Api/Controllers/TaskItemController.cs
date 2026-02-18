using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _service;

        public TaskItemController(ITaskItemService service)
        {
            _service = service;
        }

        /// <summary> 
        /// Cria uma nova tarefa. 
        /// </summary> 
        /// <param name="dto">Dados da tarefa a ser criada.</param> 
        /// <returns>Retorna a tarefa criada.</returns>
        [HttpPost("create")]
        public async Task<ActionResult<TaskItemDto>> Create([FromBody] CreateTaskItemDto dto) 
        {
            if (dto.Title is null)
                return BadRequest("Title não pode ser nulo.");
            if (dto.DueDate <= DateTime.UtcNow)
                return BadRequest("DueDate não pode ser menor que a data atual.");

            var taskItem = await _service.AddAsync(dto);

            if(taskItem is null)
                return BadRequest("Erro ao criar a tarefa.");

            return CreatedAtAction(nameof(GetById), new {id = taskItem.Id }, taskItem);
        }

        /// <summary> 
        /// Busca uma tarefa pelo ID. 
        /// </summary> 
        /// <param name="id">Identificador único da tarefa.</param> 
        /// <returns>Retorna a tarefa encontrada ou 404 se não existir.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
        {
            var taskItem = await _service.GetByIdAsync(id);

            if (taskItem is null)
                return NotFound($"Id {id}, não encontrado!");

            else
                return Ok(taskItem);
        }

        /// <summary> 
        /// Busca tarefas pelo Status. 
        /// </summary> 
        /// <param name="status">Status desejado para busca.</param> 
        /// <returns>Retorna a tarefa encontrada ou 404 se não existir.</returns>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetByStatus(StatusTask status)
        {
            var taskItems = await _service.GetByStatusAsync(status);
            return Ok(taskItems);
        }

        /// <summary> 
        /// Inicia a execução de uma tarefa existente. 
        /// </summary> 
        /// <param name="id">Identificador único da tarefa que será iniciada.</param> 
        /// <returns> 
        /// Retorna: 
        /// - 400 Bad Request se o <paramref name="id"/> não for informado (Guid.Empty); 
        /// - 404 Not Found se a tarefa não existir; 
        /// - 200 OK com os dados da tarefa atualizada quando a operação for bem-sucedida. 
        /// </returns>
        [HttpPut("{id}/start")]
        public async Task<ActionResult> Start(Guid id)
        {
            var taskItem = await _service.StartAsync(id);
            return Ok(taskItem);
        }

        /// <summary> 
        /// Pausa a execução de uma tarefa existente. 
        /// </summary> 
        /// <param name="id">Identificador único da tarefa que será pausada.</param> 
        /// <returns> 
        /// Retorna: 
        /// - 400 Bad Request se o <paramref name="id"/> não for informado (Guid.Empty); 
        /// - 404 Not Found se a tarefa não existir; 
        /// - 200 OK com os dados da tarefa atualizada quando a operação for bem-sucedida. 
        /// </returns>
        [HttpPut("{id}/on-hold")]
        public async Task<ActionResult> OnHold(Guid id)
        {
            var taskItem = await _service.OnHoldAsync(id);

            return Ok(taskItem);
        }

        /// <summary> 
        /// Completa uma tarefa existente. 
        /// </summary> 
        /// <param name="id">Identificador único da tarefa que será concluida.</param> 
        /// <returns> 
        /// Retorna: 
        /// - 400 Bad Request se o <paramref name="id"/> não for informado (Guid.Empty); 
        /// - 404 Not Found se a tarefa não existir; 
        /// - 200 OK com os dados da tarefa atualizada quando a operação for bem-sucedida. 
        /// </returns>
        [HttpPut("{id}/complete")]
        public async Task<ActionResult> Complete(Guid id)
        {
            var taskItem = await _service.CompleteAsync(id);
            return Ok(taskItem);
        }

        /// <summary> 
        /// Cancela uma tarefa existente. 
        /// </summary> 
        /// <param name="id">Identificador único da tarefa que será cancelada.</param> 
        /// <returns> 
        /// Retorna: 
        /// - 400 Bad Request se o <paramref name="id"/> não for informado (Guid.Empty); 
        /// - 404 Not Found se a tarefa não existir; 
        /// - 200 OK com os dados da tarefa atualizada quando a operação for bem-sucedida. 
        /// </returns>
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> Cancel(Guid id)
        {
            var taskItem = await _service.CancelAsync(id);
            return Ok(taskItem);
        }

    }
}
