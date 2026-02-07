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

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
        {
            var taskItem = await _service.GetByIdAsync(id);

            if (taskItem is null)
                return NotFound($"Id {id}, não encontrado!");

            else
                return Ok(taskItem);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetByStatus(StatusTask status)
        {
            var taskItems = await _service.GetByStatusAsync(status);
            return Ok(taskItems);
        }

        [HttpPut("{id}/start")]
        public async Task<ActionResult> Start(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id não informado");

            var taskItem = await _service.StartAsync(id);
            if(taskItem is null)
                return NotFound($"Id {id}, não encontrado!");
            return Ok(taskItem);
        }

        [HttpPut("{id}/on-hold")]
        public async Task<ActionResult> OnHold(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id não informado");

            var taskItem = await _service.OnHoldAsync(id);
            if (taskItem is null)
                return NotFound($"Id {id}, não encontrado!");
            return Ok(taskItem);
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult> Complete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id não informado");

            var taskItem = await _service.CompleteAsync(id);
            if (taskItem is null)
                return NotFound($"Id {id}, não encontrado!");
            return Ok(taskItem);
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> Cancel(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id não informado");

            var taskItem = await _service.CancelAsync(id);
            if (taskItem is null)
                return NotFound($"Id {id}, não encontrado!");
            return Ok(taskItem);
        }

    }
}
