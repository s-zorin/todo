using System;

namespace ToDo.Views.Shared.Components.StatusIcon
{
    public class StatusIconViewComponentModel
    {
        public DateTimeOffset DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
