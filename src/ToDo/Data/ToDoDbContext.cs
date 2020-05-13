using Microsoft.EntityFrameworkCore;

namespace ToDo.Data
{
    public sealed class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Task> Tasks { get; set; } = null!;
    }
}
