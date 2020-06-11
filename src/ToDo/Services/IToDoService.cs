using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo.Services
{
    public interface IToDoService
    {
        Task CompleteTaskAsync(string? taskId);

        Task DeleteTaskAsync(string? taskId);

        Task<IEnumerable<Models.Task>> GetSortedTasksAsync();

        Task<Models.Task> GetTaskAsync(string? taskId);

        Task<string> AddOrUpdateTaskAsync(Models.Task value);

        Task ToDoTaskAsync(string? taskId);
    }
}