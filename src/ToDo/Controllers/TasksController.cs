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
            var sortedTasks = await toDoService.GetSortedTasksAsync();

            var viewModel = new ViewModels.Tasks.Index
            {
                Tasks = sortedTasks,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Single(string? id)
        {
            var task = await toDoService.GetTaskAsync(id);

            var viewModel = new ViewModels.Tasks.Single
            {
                Task = task,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Complete(string? id)
        {
            await toDoService.CompleteTaskAsync(id);
            
            var referer = Request.Headers["Referer"];

            if (Uri.TryCreate(referer, UriKind.Absolute, out _))
            {
                return Redirect(referer);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string? id)
        {
            var task = await toDoService.GetTaskAsync(id);

            var viewModel = new ViewModels.Tasks.Edit
            {
                Task = task,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitEdit([Bind("Id", "Name", "Description", "DueDate", Prefix = "Task")] Models.Task submittedTask)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new ViewModels.Tasks.Edit
                {
                    Task = submittedTask
                };

                return View(nameof(Edit), viewModel);
            }

            var id = await toDoService.AddOrUpdateTaskAsync(submittedTask);

            return RedirectToAction(nameof(Single), new { id });
        }

        public IActionResult Create()
        {
            var task = new Models.Task
            {
                Id = null,
                Name = "New Task",
                Description = null,
                DueDate = DateTimeOffset.Now.Date,
            };

            var viewModel = new ViewModels.Tasks.Edit
            {
                Task = task,
            };

            return View(nameof(Edit), viewModel);
        }

        public async Task<IActionResult> ToDo(string? id)
        {
            await toDoService.ToDoTaskAsync(id);

            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitDelete(string? id)
        {
            await toDoService.DeleteTaskAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}