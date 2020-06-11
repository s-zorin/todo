using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public interface IToDoService
    {
        Task CompleteTaskAsync(string? taskId);

        Task DeleteTaskAsync(string? taskId);

        Task<IEnumerable<TaskModel>> GetSortedTasksAsync();

        Task<TaskModel> GetTaskAsync(string? taskId);

        Task<string> AddOrUpdateTaskAsync(TaskModel value);

        Task ToDoTaskAsync(string? taskId);
    }
}