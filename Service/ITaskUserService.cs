using TaskUserManager.DTOs;
using TaskUserManager.Models;

namespace TaskUserManager.Service
{
    public interface ITaskUserService
    {
        Task<IEnumerable<TfaUsersTask>> GetTasksByUserIdAsync(int userId);
        Task<IEnumerable<TfaTask>> GetTasksByCategoryIdAsync(int categoryId);

        Task AddAsync(TaskUserDto entity);

        Task<IEnumerable<int>> GetUserIdsByCategoryIdAsync(int categoryId);

        Task<TfaCategory?> GetLastCategoryAsync();

        Task AprobacionAsync(int userId, int userTaskId);

    }
}
