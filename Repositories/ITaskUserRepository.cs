using TaskUserManager.Models;

namespace TaskUserManager.Repositories
{
    public interface ITaskUserRepository
    {
        Task<IEnumerable<TfaUsersTask>> GetTasksByUserIdAsync(int userId);

        Task<IEnumerable<TfaTask>> GetTasksByCategoryIdAsync(int categoryId);

        Task AddAsync(TfaUsersTask entity);

        Task<IEnumerable<int>> GetUserIdsByCategoryIdAsync(int categoryId);

        Task<TfaCategory?> GetLastCategoryAsync();

        Task AddRangeAsync(IEnumerable<TfaUsersTask> entities);



    }
}
