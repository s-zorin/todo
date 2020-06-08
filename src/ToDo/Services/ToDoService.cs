using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;

namespace ToDo.Services
{
    public interface IServiceResult
    {
        bool IsOk { get; }
    }

    public interface IServiceResult<T> : IServiceResult
    {
        T Value { get; }
    }

    public class ServiceResult : IServiceResult
    {
        public bool IsOk { get; }

        public ServiceResult(bool isOk)
        {
            IsOk = isOk;
        }
    }

    public class ServiceResult<T> : ServiceResult, IServiceResult<T>
    {
        public T Value { get; }

        public ServiceResult(bool isOk, T value) : base(isOk)
        {
            Value = value;
        }
    }

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
            context.SaveChanges();

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
            context.SaveChanges();

            return new ServiceResult(true);
        }

        public async Task<IServiceResult<Models.Task>> SaveTaskAsync(string? taskId, Models.Task value)
        {
            var task = await context.FindAsync<Models.Task>(taskId);

            if (task == null)
            {
                task = new Models.Task
                {
                    Id = taskId ?? Guid.NewGuid().ToString(),
                    Name = value.Name,
                    Description = value.Description,
                    DueDate = value.DueDate,
                };

                context.Add(task);
            }
            else
            {
                task.Name = value.Name;
                task.Description = value.Description;
                task.DueDate = value.DueDate;
            }

            context.SaveChanges();

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
            context.SaveChanges();

            return new ServiceResult(true);
        }
    }
}