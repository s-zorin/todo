using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Extensions;

namespace ToDo.Services
{

    public class ToDoService : IToDoService
    {
        private readonly ToDoDbContext context;

        public ToDoService(ToDoDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Models.Task>> GetSortedTasksAsync()
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

            return tasks;
        }

        public async Task<Models.Task> GetTaskAsync(string? taskId)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            if (task == null)
            {
                throw new InvalidOperationException("Task with given ID was not found.");
            }

            return task;
        }

        public async Task CompleteTaskAsync(string? taskId)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            if (task == null)
            {
                throw new InvalidOperationException("Can not move task into Completed state. Task with given ID was not found.");
            }

            task.IsCompleted = true;
            await context.SaveChangesAsync();
        }

        public async Task ToDoTaskAsync(string? taskId)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            if (task == null)
            {
                throw new InvalidOperationException("Can not move task into ToDo state. Task with given ID was not found.");
            }

            task.IsCompleted = false;
            await context.SaveChangesAsync();
        }

        public async Task<string> AddOrUpdateTaskAsync(Models.Task task)
        {
            var entry = await context.AddOrUpdateAsync(task);
            await context.SaveChangesAsync();
            return entry.Entity.Id!;
        }

        public async Task DeleteTaskAsync(string? taskId)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            if (task == null)
            {
                throw new InvalidOperationException("Can not delete task. Task with given ID was not found.");
            }

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();
        }
    }
}