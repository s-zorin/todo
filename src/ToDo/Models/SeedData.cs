﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;

namespace ToDo.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ToDoDbContext(serviceProvider.GetRequiredService<DbContextOptions<ToDoDbContext>>()))
            {
                if (context.Tasks.Any())
                {
                    return;
                }

                context.Tasks.Add(new Models.Task
                {
                    Name = "Water the flowers",
                    DueDate = DateTimeOffset.Now.AddDays(1).AddHours(5),
                    IsCompleted = false
                });

                context.Tasks.Add(new Models.Task
                {
                    Name = "Feed the cat",
                    DueDate = DateTimeOffset.Now.AddHours(8),
                    IsCompleted = false
                });

                context.Tasks.Add(new Models.Task
                {
                    Name = "Read a book",
                    DueDate = DateTimeOffset.Now.AddHours(-20),
                    Description = "Maybe some sci-fi.",
                    IsCompleted = true
                });

                context.Tasks.Add(new Models.Task
                {
                    Name = "Tidy up",
                    DueDate = DateTimeOffset.Now.AddHours(1),
                    Description = "What is this? A man cave?",
                    IsCompleted = true
                });

                context.SaveChanges();
            }
        }
    }
}
