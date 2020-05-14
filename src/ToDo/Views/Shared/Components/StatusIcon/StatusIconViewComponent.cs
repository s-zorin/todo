using Microsoft.AspNetCore.Mvc;
using System;

namespace ToDo.Views.Shared.Components.StatusIcon
{
    public class StatusIconViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DateTimeOffset dueDate, bool isCompleted)
        {
            var model = new StatusIconViewComponentModel
            {
                DueDate = dueDate,
                IsCompleted = isCompleted,
            };

            return View(model);
        }
    }
}
