using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HtmlTemplate = System.Func<dynamic?, Microsoft.AspNetCore.Html.IHtmlContent>;

namespace ToDo.Views.Shared.Components.Modal
{
    public class ModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string id, string? title, string? message, IEnumerable<HtmlTemplate> actions)
        {
            var model = new ModalViewComponentModel
            {
                Id = id,
                Title = title,
                Message = message,
                Actions = actions
            };

            return View(model);
        }
    }
}
