using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDo.Data;

namespace ToDo.Controllers
{
    public class TasksController : Controller
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
                return NotFound("Value?");
            }

            var viewModel = new ToDo.ViewModels.Tasks.Single
            {
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
            };

            return View(viewModel);
        }
    }
}