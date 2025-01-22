using TaskUserManager.DTOs;
using TaskUserManager.Models;

namespace TaskUserManager.Service
{
    public interface ITaskUserService
    {
        Task<IEnumerable<TfaUsersTask>> GetTasksByUserIdAsync(int userId);
        Task<IEnumerable<TfaTask>> GetTasksByCategoryIdAsync(int categoryId);

        Task AddAsync(int TaskId);

        Task<IEnumerable<int>?> GetUserIdsByCategoryIdAsync(int categoryId);

        Task<TfaCategory?> GetLastCategoryAsync();

        Task AprobacionAsync(int userId, int userTaskId);

        Task SubirImagenTarea(UpdateEvidence updateEvidence);

        Task DeleteTaskAsync(int taskId);

        //Task<TfaTask> GetTaskAsync(int id);

        Task<List<TfaUser>> GetUserByCategories(int id);

        Task<List<TfaTask>> GetTaskByCategories(int userid);

    }
}
