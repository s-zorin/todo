using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Models
{
    public class Task
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
