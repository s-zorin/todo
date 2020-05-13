using System;

namespace ToDo.ViewModels.Tasks
{
    public sealed class Single
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
