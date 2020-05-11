using System;

namespace ToDo.ViewModels.Tasks
{
    public class Single
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
