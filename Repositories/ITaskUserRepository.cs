using TaskUserManager.DTOs;
using TaskUserManager.Models;

namespace TaskUserManager.Repositories
{
    public interface ITaskUserRepository
    {
        Task<IEnumerable<TfaUsersTask>> GetTasksByUserIdAsync(int userId);

        Task<IEnumerable<TfaTask>> GetTasksByCategoryIdAsync(int categoryId);

        Task AddAsync(TfaUsersTask entity);

        Task<IEnumerable<int>?> GetUserIdsByCategoryIdAsync(int categoryId);

        Task<TfaCategory?> GetLastCategoryAsync();

        Task AddRangeAsync(IEnumerable<TfaUsersTask> entities);

        Task AprobacionAsync(int userId, int userTaskId);

        Task SubirImagenTarea(UpdateEvidence updateEvidence);

        Task<TfaTask> GetTaskAsync(int id);
        Task DeleteTaskAsync(int taskId);

        Task<List<TfaUser>> GetUserByCategories(int id);

    }
}
