using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDo.Services;

namespace ToDo.Controllers
{
    public sealed class TasksController : Controller
    {
        private readonly IToDoService toDoService;

        public TasksController(IToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await toDoService.GetSortedTasksAsync();

            if (!result.IsOk)
            {
                return BadRequest();
            }

            var viewModel = new ViewModels.Tasks.Index
            {
                Tasks = result.Value
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Single(string? id)
        {
            var result = await toDoService.GetTaskAsync(id);

            if (!result.IsOk)
            {
                return BadRequest();
            }

            var viewModel = new ViewModels.Tasks.Single
            {
                Task = result.Value,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Complete(string? id)
        {
            var result = await toDoService.CompleteTaskAsync(id);

            if (!result.IsOk)
            {
                return BadRequest();
            }
            
            var referer = Request.Headers["Referer"];

            if (Uri.TryCreate(referer, UriKind.Absolute, out _))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Tasks");
        }

        public async Task<IActionResult> Edit(string? id)
        {
            var result = await toDoService.GetTaskAsync(id);

            if (!result.IsOk)
            {
                return BadRequest();
            }

            var viewModel = new ViewModels.Tasks.Edit
            {
                Task = result.Value,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitEdits(string? id, [Bind("Name", "Description", "DueDate", Prefix = "Task")] Models.Task submittedTask)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new ViewModels.Tasks.Edit
                {
                    Task = submittedTask
                };

                return View("Edit", viewModel);
            }

            var result = await toDoService.SaveTaskAsync(id, submittedTask);

            if (!result.IsOk)
            {
                return BadRequest();
            }

            return RedirectToAction("Single", "Tasks", new { result.Value.Id });
        }

        public IActionResult Create()
        {
            var task = new Models.Task
            {
                Name = "New Task",
                Description = null,
                DueDate = DateTimeOffset.Now.Date,
            };

            var viewModel = new ViewModels.Tasks.Edit
            {
                Task = task,
            };

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> ToDo(string? id)
        {
            var result = await toDoService.ToDoTaskAsync(id);

            if (!result.IsOk)
            {
                return BadRequest();
            }

            return RedirectToAction("Edit", "Tasks", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitDelete(string? id)
        {
            var result = await toDoService.DeleteTaskAsync(id);

            if (!result.IsOk)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Tasks");
        }
    }
}