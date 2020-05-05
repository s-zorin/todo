using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ToDo.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}