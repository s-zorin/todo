using System.Collections.Generic;

namespace ToDo.ViewModels.Tasks
{
    public sealed class Index
    {
        public IEnumerable<Models.Task> Tasks { get; set; } = new List<Models.Task>();
    }
}