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

        Task<List<TfaTask>> gettaskbyuser(int id);
        Task<List<TfaTask>> GetTaskByCategories(int id);
        Task<List<TfaUser>> GetUserByCategories(int id);
    }
}
