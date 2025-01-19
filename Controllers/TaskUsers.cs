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
            return Ok(tasks);
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
    }
}
