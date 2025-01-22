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

        private readonly IFileUploadService _fileUploadService;
        public TaskUsers(ITaskUserService service, IFileUploadService fileUploadService)
        {
            _service = service;
            _fileUploadService = fileUploadService;
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

        [HttpPost("AsignarTareas")]
        public async Task<IActionResult> AddTaskUser(int TaskId)
        {
            await _service.AddAsync(TaskId);
            return Ok();
        }

        [HttpPut("Aprobaciones")]
        public async Task<IActionResult> AprobarTarea(int userId, int userTaskId)
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

        [HttpPut("SubirImagen")]

        public async Task<IActionResult> SubirImagen(UpdateEvidence updateEvidenceDto)
        {
            if (updateEvidenceDto.vlf_image == null)
            {
                return BadRequest("El archivo no puede ser nulo.");
            }
            try
            {
                var filePath = await _fileUploadService.UploadUserImageAsync(updateEvidenceDto.vlf_image);
                await _service.SubirImagenTarea(updateEvidenceDto);
                return Ok("La imagen se subió correctamente.");
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

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                await _service.DeleteTaskAsync(taskId);
                return Ok("La tarea se eliminó correctamente.");
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

        [HttpGet("GetUserByCategories")]

        public async Task<IActionResult> GetUserByCategories(int idCategory)
        {
            var users = await _service.GetUserByCategories(idCategory);

            if (users == null || users.Count == 0)
            {
                return NotFound(new { message = "No se encontraron usuarios para la categoría proporcionada" });
            }

            return Ok(users);

        }

        [HttpGet("GetTaskByCategories")]

        public async Task<IActionResult> GetTaskByCategories(int userId)
        {
            var tasks = await _service.GetTaskByCategories(userId);

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound(new { message = "No se encontraron tareas para el usuario proporcionado" });
            }
            return Ok(tasks);
        }


    }
}
