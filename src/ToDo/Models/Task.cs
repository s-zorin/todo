using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public sealed class Task : IValidatableObject
    {
        public string? Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsOverdue => DueDate < DateTimeOffset.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DueDate < DateTimeOffset.Now)
            {
                yield return new ValidationResult("Due date can not be in the past.");
            }
        }
    }
}
