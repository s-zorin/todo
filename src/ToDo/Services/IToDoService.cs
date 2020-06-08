using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDo.Services
{
    public interface IToDoService
    {
        Task<IServiceResult> CompleteTaskAsync(string? taskId);

        Task<IServiceResult> DeleteTaskAsync(string? taskId);

        Task<IServiceResult<IEnumerable<Models.Task>>> GetSortedTasksAsync();

        Task<IServiceResult<Models.Task>> GetTaskAsync(string? taskId);

        Task<IServiceResult<Models.Task>> SaveTaskAsync(string? taskId, Models.Task value);

        Task<IServiceResult> ToDoTaskAsync(string? taskId);
    }
}