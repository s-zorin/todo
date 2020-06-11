using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDo.Models
{
    [Table("Tasks")]
    public sealed class TaskModel : IValidatableObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsOverdue => DueDate < DateTimeOffset.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DueDate.Date < DateTimeOffset.Now.Date)
            {
                yield return new ValidationResult("Due date can not be in the past.");
            }
        }
    }
}
