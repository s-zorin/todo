using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDo.Data;

namespace ToDo.Controllers
{
    public sealed class TasksController : Controller
    {
        private readonly ToDoDbContext context;

        public TasksController(ToDoDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var now = DateTimeOffset.Now;

            var viewModel = new ToDo.ViewModels.Tasks.Index
            {
                // Sort tasks as Overdue > ToDo > Completed. Then sort each group by due date.
                Tasks = context.Tasks
                .Select(t => new { Priority = t.IsCompleted ? 2 : t.DueDate < now ? 0 : 1, Value = t })
                .OrderBy(t => t.Priority)
                .ThenBy(t => t.Value.DueDate)
                .Select(t => t.Value)
                .ToList()
            };

            return View(viewModel);
        }

        public IActionResult Single(string? id)
        {
            var task = context.Find<ToDo.Models.Task>(id);

            if (task == null)
            {
                return NotFound("Task not found.");
            }

            var viewModel = new ToDo.ViewModels.Tasks.Single
            {
                Task = task,
            };

            return View(viewModel);
        }

        public IActionResult Complete(string? id)
        {
            var task = context.Find<ToDo.Models.Task>(id);

            if (task == null)
            {
                return NotFound("Task not found.");
            }

            task.IsCompleted = true;
            context.SaveChanges();
            
            var referer = Request.Headers["Referer"];

            if (Uri.TryCreate(referer, UriKind.Absolute, out _))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Tasks");
        }

        public IActionResult Edit(string? id)
        {
            var task = context.Find<ToDo.Models.Task>(id);

            if (task == null)
            {
                return NotFound("Task not found.");
            }

            var viewModel = new ToDo.ViewModels.Tasks.Edit
            {
                Task = task,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitEdits(string? id, [Bind("Name", "Description", "DueDate", Prefix = "Task")] Models.Task submittedTask)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", new ToDo.ViewModels.Tasks.Edit
                {
                    Task = submittedTask
                });
            }

            var task = context.Find<ToDo.Models.Task>(id);

            if (task == null)
            {
                task = new ToDo.Models.Task
                {
                    Id = id ?? Guid.NewGuid().ToString(),
                    Name = submittedTask.Name,
                    Description = submittedTask.Description,
                    DueDate = submittedTask.DueDate,
                };

                context.Add(task);
            }
            else
            {
                task.Name = submittedTask.Name;
                task.Description = submittedTask.Description;
                task.DueDate = submittedTask.DueDate;
            }

            context.SaveChanges();

            return RedirectToAction("Single", "Tasks", new { task.Id });
        }

        public IActionResult Create()
        {
            var task = new ToDo.Models.Task
            {
                Name = "New Task",
                Description = null,
                DueDate = DateTimeOffset.Now.Date,
            };

            var viewModel = new ToDo.ViewModels.Tasks.Edit
            {
                Task = task,
            };

            return View("Edit", viewModel);
        }

        public IActionResult ToDo(string? id)
        {
            var task = context.Find<ToDo.Models.Task>(id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            task.IsCompleted = false;

            context.SaveChanges();

            return RedirectToAction("Edit", "Tasks", new { task.Id });
        }

        [HttpPost]
        public IActionResult SubmitDelete(string? id)
        {
            var task = context.Find<ToDo.Models.Task>(id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            context.Tasks.Remove(task);
            context.SaveChanges();

            return RedirectToAction("Index", "Tasks");
        }
    }
}