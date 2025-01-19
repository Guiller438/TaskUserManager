using Microsoft.AspNetCore.Mvc;
using TaskUserManager.DTOs;
using TaskUserManager.Models;
using TaskUserManager.Service;

namespace TaskUserManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskUsers : ControllerBase
    {
        private readonly ITaskUserService _service;
        public TaskUsers(ITaskUserService service)
        {
            _service = service;
        }

        [HttpGet("tasks/{userId}")]
        public async Task<IActionResult> GetTasksByUserId(int userId)
        {
            var tasks = await _service.GetTasksByUserIdAsync(userId);
            var taskDtos = tasks.Select(task => new TaskUserDto
            {
                UserTaskId = task.UserTaskId,
                UserId = task.UserId,
                StatusTask = task.StatusTask,
                EvidencePath = task.EvidencePath,
                EvidenceUploadDate = task.EvidenceUploadDate
            });

            return Ok(taskDtos);
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskUser([FromBody] TaskUserDto taskUser)
        {
            if (taskUser == null)
            {
                return BadRequest("La tarea del usuario no puede ser nula.");
            }

            await _service.AddAsync(taskUser);
            return Ok(taskUser);
        }

        [HttpPut("Aprobaciones")]
        public async Task<IActionResult> UpdateTask(int userId, int userTaskId)
        {

            try
            {
                await _service.AprobacionAsync(userId, userTaskId);
                return Ok("La tarea se actualizó correctamente.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
