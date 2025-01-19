using Microsoft.EntityFrameworkCore;
using TaskUserManager.Data;
using TaskUserManager.Models;

namespace TaskUserManager.Repositories
{
    public class TaskUserRepository : ITaskUserRepository
    {
        private readonly DbAb0bdeTalentseedsContext _context;

        public TaskUserRepository(DbAb0bdeTalentseedsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TfaUsersTask>> GetTasksByUserIdAsync(int userId)
        {
            return await _context.TfaUsersTasks
                .Include(t => t.User)
                .Include(t => t.UserTask)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUserIdsByCategoryIdAsync(int categoryId)
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
    }
}
