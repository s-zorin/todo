using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Services.Results;

namespace ToDo.Services
{

    public class ToDoService : IToDoService
    {
        private readonly ToDoDbContext context;

        public ToDoService(ToDoDbContext context)
        {
            this.context = context;
        }

        public async Task<IServiceResult<IEnumerable<Models.Task>>> GetSortedTasksAsync()
        {
            // Making EF happy.
            var now = DateTimeOffset.Now;

            // Sort tasks as Overdue > ToDo > Completed. Then sort each group by due date.
            var tasks = await context.Tasks
                .Select(t => new { Priority = t.IsCompleted ? 2 : t.DueDate < now ? 0 : 1, Value = t })
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.Value.DueDate)
                .Select(t => t.Value)
                .ToListAsync();

            return new ServiceResult<IEnumerable<Models.Task>>(true, tasks);
        }

        public async Task<IServiceResult<Models.Task>> GetTaskAsync(string? taskId)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            return new ServiceResult<Models.Task>(true, task);
        }

        public async Task<IServiceResult> CompleteTaskAsync(string? taskId)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            if (task == null)
            {
                return new ServiceResult(false);
            }

            task.IsCompleted = true;
            await context.SaveChangesAsync();

            return new ServiceResult(true);
        }

        public async Task<IServiceResult> ToDoTaskAsync(string? taskId)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            if (task == null)
            {
                return new ServiceResult(false);
            }

            task.IsCompleted = false;
            await context.SaveChangesAsync();

            return new ServiceResult(true);
        }

        public async Task<IServiceResult<Models.Task>> SaveTaskAsync(string? taskId, Models.Task task)
        {
            var existingTask = await context.FindAsync<Models.Task>(taskId);

            if (existingTask == null)
            {
                await SaveNewTaskAsync(task);
            }
            else
            {
                await UpdateExistingTaskAsync(existingTask, task);
            }

            return new ServiceResult<Models.Task>(true, task);
        }

        public async Task<IServiceResult> DeleteTaskAsync(string? taskId)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            if (task == null)
            {
                return new ServiceResult(false);
            }

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            return new ServiceResult(true);
        }

        private async Task SaveNewTaskAsync(Models.Task task)
        {
            var newTask = new Models.Task
            {
                Id = Guid.NewGuid().ToString(),
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate,
            };

            context.Add(newTask);
            await context.SaveChangesAsync();
        }

        private async Task UpdateExistingTaskAsync(Models.Task existingTask, Models.Task task)
        {
            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            await context.SaveChangesAsync();
        }
    }
}