using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskUserManager.Data;
using TaskUserManager.DTOs;
using TaskUserManager.Models;
using TaskUserManager.Service;

namespace TaskUserManager.Repositories
{
    public class TaskUserRepository : ITaskUserRepository
    {
        private readonly DbAb0bdeTalentseedsContext _context;

        private readonly IFileUploadService _fileUploadService;

        public TaskUserRepository(DbAb0bdeTalentseedsContext context, IFileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
        }

        public async Task<IEnumerable<TfaUsersTask>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.TfaUsersTasks
                .Include(t => t.User)
                .Include(t => t.UserTask)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>?> GetUserIdsByCategoryIdAsync(int categoryId)
        {

            var teamIds = await _context.TfaTeamsCategories
                .Where(ctc => ctc.CategoriesId == categoryId) // Filtra por categoría
                .Select(ctc => ctc.TeamId) // Obtén solo los IDs de los equipos
                .ToListAsync();

            var collaboratorIds = await _context.TfaTeamsColaborators
                .Where(tc => teamIds.Contains(tc.ColaboratorTeamID)) // Filtra por los TeamId obtenidos
                .Select(tc => tc.ColaboratorUsersID) // Obtén los IDs de los colaboradores
                .Distinct() // Evita duplicados
                .ToListAsync();

            return collaboratorIds;

        }

        public async Task<IEnumerable<TfaTask>> GetTasksByCategoryIdAsync(int categoryId)
        {
            return await _context.TfaTasks
                .Where(t => t.CategoryId == categoryId)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task AddAsync(TfaUsersTask entity)
        {
            await _context.TfaUsersTasks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<TfaCategory?> GetLastCategoryAsync()
        {
            return await _context.TfaCategories
                                    .OrderByDescending(c => c.CategoryId) // O usa CreatedDate si está disponible
                                    .FirstOrDefaultAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TfaUsersTask> entities)
        {
            _context.TfaUsersTasks.AddRange(entities); // _context es tu DbContext
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
        }

        public async Task AprobacionAsync(int userId, int userTaskId)
        {
            // Busca la entidad existente en la base de datos
            var existingTask = await _context.TfaUsersTasks
                .FirstOrDefaultAsync(t => t.UserId == userId && t.UserTaskId == userTaskId);

            // Si no se encuentra, lanza una excepción o maneja el error
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"No se encontró una tarea para el usuario con ID {userId}.");
            }

            // Actualiza los campos necesarios
            existingTask.StatusTask = true;

            // Marca la entidad como modificada y guarda los cambios
            _context.TfaUsersTasks.Update(existingTask);
            await _context.SaveChangesAsync();
        }


        public async Task SubirImagenTarea(UpdateEvidence updateEvidence)
        {
            // Validar que el archivo no sea nulo
            if (updateEvidence.vlf_image == null)
            {
                throw new ArgumentException("El archivo no puede ser nulo.");
            }

            // Buscar la tarea en la base de datos usando los IDs proporcionados
            var existingTask = await _context.TfaUsersTasks
                .FirstOrDefaultAsync(t => t.UserId == updateEvidence.UserId && t.UserTaskId == updateEvidence.UserTaskId);

            // Si no se encuentra, lanza una excepción para notificar al controlador
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"No se encontró una tarea para el usuario con ID {updateEvidence.UserId} y tarea con ID {updateEvidence.UserTaskId}.");
            }

            // Subir la imagen usando el servicio de subida de archivos
            var filePath = await _fileUploadService.UploadUserImageAsync(updateEvidence.vlf_image);

            // Actualizar la ruta de la evidencia y la fecha de subida
            existingTask.EvidencePath = filePath;
            existingTask.EvidenceUploadDate = updateEvidence.EvidenceUploadDate ?? DateTime.Now;

            // Marca la entidad como modificada y guarda los cambios
            _context.TfaUsersTasks.Update(existingTask);
            await _context.SaveChangesAsync();
        }

        public async Task<TfaTask> GetTaskAsync(int id)
        {
            var taskEntity = await _context.TfaTasks.FindAsync(id);

            if (taskEntity == null)
            {
                throw new KeyNotFoundException($"No se encontró la tarea con el ID {id}.");
            }

            return taskEntity;
        }


        public async Task DeleteTaskAsync(int id)
        {
            // Buscar todos los registros que coincidan con el TaskId
            var taskUserEntities = await _context.TfaUsersTasks
                                             .Where(t => t.UserTaskId == id)
                                             .ToListAsync();


            if (taskUserEntities == null || !taskUserEntities.Any())
            {
                throw new KeyNotFoundException($"No se encontraron tareas con el ID {id}.");
            }

            // Eliminar los registros encontrados
            _context.TfaUsersTasks.RemoveRange(taskUserEntities);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();
        }

        public async Task<List<TfaUser>> GetUserByCategories(int id)
        {
            try
            {
                // Verificar si existen equipos para la categoría antes de obtener los IDs
                var teamIds = await _context.TfaTeamsCategories
                    .Where(ctc => ctc.CategoriesId == id)
                    .Select(ctc => ctc.TeamId)
                    .ToListAsync();

                if (!teamIds.Any())
                {
                    return new List<TfaUser>();  // Retorna lista vacía si no hay equipos
                }

                // Obtener IDs de colaboradores asociados a los equipos filtrados
                var collaboratorIds = await _context.TfaTeamsColaborators
                    .Where(tc => teamIds.Contains(tc.ColaboratorTeamID))
                    .Select(tc => tc.ColaboratorUsersID)
                    .Distinct()
                    .ToListAsync();

                if (!collaboratorIds.Any())
                {
                    return new List<TfaUser>();  // Retorna lista vacía si no hay colaboradores
                }

                // Obtener lista de usuarios usando los IDs de colaboradores
                return await _context.TfaUsers
                    .Where(u => collaboratorIds.Contains(u.UsersId))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log del error (puedes usar una librería como Serilog o simplemente registrar en consola/log)
                Console.WriteLine($"Error obteniendo usuarios por categoría: {ex.Message}");
                throw new ApplicationException("Error al obtener los usuarios por categoría", ex);
            }
        }


        public async Task<List<TfaTask>> GetTaskByCategories(int userid)
        {
            try
            {
                // Verificar si existen equipos para la categoría antes de obtener los IDs
                var categoriesIds = await _context.TfaTeamsCategories
                    .Where(ctc => ctc.CategoriesId == userid)
                    .Select(ctc => ctc.CategoriesId)
                    .ToListAsync();

                if (!categoriesIds.Any())
                {
                    return new List<TfaTask>();  // Retorna lista vacía si no hay equipos
                }

                // Obtener lista de usuarios usando los IDs de colaboradores
                var tasks = await _context.TfaTasks
                    .Where(u => categoriesIds.Contains(u.CategoryId.Value))
                    .ToListAsync();

                if (!tasks.Any())
                {
                    return new List<TfaTask>();  // Retorna lista vacía si no hay colaboradores
                }

                return new List<TfaTask>(tasks);

            }
            catch (Exception ex)
            {
                // Log del error (puedes usar una librería como Serilog o simplemente registrar en consola/log)
                Console.WriteLine($"Error obteniendo usuarios por categoría: {ex.Message}");
                throw new ApplicationException("Error al obtener los usuarios por categoría", ex);
            }
        }


    }
}
