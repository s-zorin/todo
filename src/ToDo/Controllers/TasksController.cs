using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ToDo.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new ToDo.ViewModels.Tasks.Index
            {
                Tasks = new[]
                {
                    "1",
                    "2",
                    "3",
                }
            };

            return View(viewModel);
        }

        public IActionResult Single(string id)
        {
            var viewModel = new ToDo.ViewModels.Tasks.Single
            {
                Name = "Nam",
                Description = "Desc",
                DueDate = DateTime.Now.AddDays(1),
                IsCompleted = false,
            };

            return View(viewModel);
        }
    }
}