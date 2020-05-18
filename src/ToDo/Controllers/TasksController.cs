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
            var task = context.Find<ToDo.Models.Task>(id);

            if (task == null)
            {
                return RedirectToAction("Index", "Tasks");
            }

            task.Name = submittedTask.Name;
            task.Description = submittedTask.Description;
            task.DueDate = submittedTask.DueDate;

            context.SaveChanges();

            var viewModel = new ToDo.ViewModels.Tasks.Single
            {
                Task = task
            };

            return View("Single", viewModel);
        }
    }
}