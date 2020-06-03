using System.Collections.Generic;
using HtmlTemplate = System.Func<dynamic?, Microsoft.AspNetCore.Html.IHtmlContent>;

namespace ToDo.Views.Shared.Components.Modal
{
    public class ModalViewComponentModel
    {
        public string Id { get; set; } = string.Empty;

        public string? Title { get; set; }

        public string? Message { get; set; }

        public IEnumerable<HtmlTemplate> Actions { get; set; } = new List<HtmlTemplate>();
    }
}
