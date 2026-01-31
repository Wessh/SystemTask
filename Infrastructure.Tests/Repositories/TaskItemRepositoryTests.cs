using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Repositories
{
    public class TaskItemRepositoryTests
    {
        [Fact]
        public async Task AddAsync_WhenAddNewTask_ShouldPersistTask()
        {
            // Arrange
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            var repository = new TaskItemRepository(context);
            var task = new TaskItem("Título", "Descrição", DateTime.UtcNow.AddDays(1));

            // Act
            await repository.AddAsync(task);
            var savedTask = await repository.GetByIdAsync(task.Id);

            // Assert
            Assert.NotNull(savedTask);
            Assert.Equal("Título", savedTask.Title);
            Assert.Equal("Descrição", savedTask.Description);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTaskExists_ShouldReturnTask()
        {
            
            // Arrange
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            var repository = new TaskItemRepository(context);

            var task = new TaskItem("Título", "Descrição", DateTime.UtcNow.AddDays(1));
            await repository.AddAsync(task);

            // Act
            var savedTask = await repository.GetByIdAsync(task.Id);
            
            // Assert
            Assert.NotNull(savedTask);
            Assert.Equal("Título", savedTask.Title); 
            Assert.Equal("Descrição", savedTask.Description);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTaskDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            var repository = new TaskItemRepository(context);

            // Act
            var savedTask = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(savedTask);
        }

        [Fact]
        public async Task UpdateAsync_WhenTaskIsValid_ShouldUpdateTask()
        {
            // Arrange
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            var repository = new TaskItemRepository(context);

            var task = new TaskItem("Título", "Descrição", DateTime.UtcNow.AddDays(1));
            await repository.AddAsync(task);

            // Act
            task.StartTask();
            await repository.UpdateAsync(task);
            var taskTest = await repository.GetByIdAsync(task.Id);

            // Assert
            Assert.NotNull(taskTest);
            Assert.Equal("Título", taskTest.Title); Assert.Equal("Descrição", taskTest.Description);
            Assert.Equal(StatusTask.InProgress, taskTest!.Status);
        }

        [Fact]
        public async Task GetByStatusAsync_WhenTasksExistWithStatus_ShouldReturnTasks()
        {
            // Arrange
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            var repository = new TaskItemRepository(context);

            var task1 = new TaskItem("Título 01", "Descrição 01", DateTime.UtcNow.AddDays(1));
            var task2 = new TaskItem("Título 02", "Descrição 02", DateTime.UtcNow.AddDays(2));
            var task3 = new TaskItem("Título 03", "Descrição 03", DateTime.UtcNow.AddDays(3));
            
            await repository.AddAsync(task1);
            await repository.AddAsync(task2);
            await repository.AddAsync(task3);

            // Act
            task1.StartTask();
            task2.CancelTask();
            task3.StartTask();

            await repository.UpdateAsync(task1);
            await repository.UpdateAsync(task2);
            await repository.UpdateAsync(task3);

            var inProgressTasks = await repository.GetByStatusAsync(StatusTask.InProgress);

            // Asserts
            Assert.NotNull(inProgressTasks); 
            Assert.Equal(2, inProgressTasks.Count()); 
            Assert.All(inProgressTasks, t => Assert.Equal(StatusTask.InProgress, t.Status));
            Assert.Equal(StatusTask.InProgress, inProgressTasks.First().Status); 
            Assert.Equal("Título 01", inProgressTasks.First().Title);
        }

    }
}
