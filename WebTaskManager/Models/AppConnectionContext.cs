using Microsoft.EntityFrameworkCore;
using System;

namespace WebTaskManager.Models
{
    public class AppConnectionContext : DbContext
    {
        public AppConnectionContext(DbContextOptions<AppConnectionContext> options) :
            base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<WorkTask> WorkTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminRole = new Role() { Id = 1, Name = "admin" };
            var userRole = new Role() { Id = 2, Name = "user" };

            var admin = new User() { Id = 1, Name = "admin", Password = "12345", RoleId = adminRole.Id };
            var user = new User() { Id = 2, Name = "user", Password = "12345", RoleId = userRole.Id };

            var task1 = new WorkTask() { Id = 1, Name = "Task 1", Body = "Do something 1", DateCreated = DateTime.Now };
            var task2 = new WorkTask() { Id = 2, Name = "Task 2", Body = "Do something 2", DateCreated = DateTime.Now };
            var task3 = new WorkTask() { Id = 3, Name = "Task 3", Body = "Do something 3", DateCreated = DateTime.Now };
            var task4 = new WorkTask() { Id = 4, Name = "Task 4", Body = "Do something 4", DateCreated = DateTime.Now };
            var task5 = new WorkTask() { Id = 5, Name = "Task 5", Body = "Do something 5", DateCreated = DateTime.Now };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { admin, user });
            modelBuilder.Entity<WorkTask>().HasData(new WorkTask[] { task1, task2, task3, task4, task5});

            base.OnModelCreating(modelBuilder);
        }
    }
}
