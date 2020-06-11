using System.Collections.Generic;
using ToDo.Models;

namespace ToDo.ViewModels.Tasks
{
    public sealed class IndexViewModel
    {
        public IEnumerable<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}