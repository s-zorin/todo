using Microsoft.AspNetCore.Mvc;

namespace ToDo.Views.Shared.Components.StatusIcon
{
    public class StatusIconViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(bool isOverdue, bool isCompleted)
        {
            var model = new StatusIconViewComponentModel
            {
                IsOverdue = isOverdue,
                IsCompleted = isCompleted,
            };

            return View(model);
        }
    }
}
