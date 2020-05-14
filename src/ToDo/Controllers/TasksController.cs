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
            var viewModel = new ToDo.ViewModels.Tasks.Index
            {
                Tasks = context.Tasks.ToList()
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
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
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
    }
}