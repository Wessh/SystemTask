using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Tests.Entities
{
    [Trait("Entity", "TaskItemTests")]
    public class TaskItemTests
    {

        #region Constructor Tests
        [Fact]
        public void Contructor_ShouldCreateTask_WithPendingStatus() 
        {
            // Arrange
            var title = "Title";
            var description = "Description";
            var dueDate = DateTime.UtcNow.AddDays(1);

            //Act
            var newTask = new TaskItem(title, description, dueDate);

            // Asserts
            Assert.Equal(StatusTask.Pending, newTask.Status);
            Assert.Equal(title, newTask.Title);
            Assert.Equal(description, newTask.Description);
            Assert.True(newTask.DueDate >= DateTime.UtcNow);
        }
        #endregion

        #region ChangeStatus Tests
        [Fact]
        public void ChangeStatus_WhenValidStatus_ShouldUpdateStatus()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));
            var newStatus = StatusTask.InProgress;

            // Act
            newTask.ChangeStatus(newStatus);

            // Asserts
            Assert.Equal(StatusTask.InProgress, newTask.Status);
        }

        [Fact]
        public void ChangeStatus_WhenCancelledStatus_ShouldNotUpdateStatus()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));
            
            // Act
            newTask.ChangeStatus(StatusTask.Cancelled);
            var act = () => newTask.ChangeStatus(StatusTask.InProgress);
            
            // Asserts
            var exception = Assert.Throws<InvalidOperationException>(() => act());
            Assert.Equal("Task cannot change status", exception.Message);
        }
        #endregion

        #region StartTask Tests
        [Fact]
        public void StartTask_WhenTaskIsPending_ShouldSetTaskToInProgress()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));

            // Act
            newTask.StartTask();

            // Asserts
            Assert.Equal(StatusTask.InProgress, newTask.Status);
        }

        [Fact]
        public void StartTask_WhenTaskNotIsPending_ShouldNotChangeStatus()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));

            // Act
            newTask.StartTask();
            newTask.CompleteTask();
            var act = () => newTask.StartTask();

            // Asserts
            var exception = Assert.Throws<InvalidOperationException>(() => act());
            Assert.Equal("Task cannot be started", exception.Message);
        }
        #endregion

        #region OnHoldTask Tests
        [Fact]
        public void OnHoldTask_WhenTaskInProgress_ShouldTaskToOnHold()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));

            // Act
            newTask.StartTask();
            newTask.OnHoldTask();

            // Asserts
            Assert.Equal(StatusTask.OnHold, newTask.Status);
        }

        [Fact]
        public void OnHoldTask_WhenTaskNotInProgress_ShouldTaskNotChangeStatus()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));

            // Act
            var act = () => newTask.OnHoldTask();

            // Asserts
            var exception = Assert.Throws<InvalidOperationException>(() => act());
            Assert.Equal("Task cannot on hold", exception.Message);
        }
        #endregion

        #region CompleteTask Tests
        [Fact]
        public void CompleteTask_WhenInProgress_ShouldSetTaskToComplete()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));

            // Act
            newTask.StartTask();
            newTask.CompleteTask();

            // Asserts
            Assert.Equal(StatusTask.Completed, newTask.Status);
        }

        [Fact]
        public void CompleteTask_WhenNotInProgress_ShouldNotChangeStatus()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));

            // Act
            var act = () => newTask.CompleteTask();

            // Asserts
            var exception = Assert.Throws<InvalidOperationException>(() => act());
            Assert.Equal("Task status cannot be completed", exception.Message);
        }
        #endregion

        #region Cancelled Tests
        [Fact]
        public void CancelledTest_WhenIsPending_ShouldSetTaskToCancelled()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));

            // Act
            newTask.StartTask();
            newTask.CancelTask();

            // Asserts
            Assert.Equal(StatusTask.Cancelled, newTask.Status);
        }

        [Fact]
        public void CancelledTest_WhenIsCompleted_ShouldNotChangeStatus()
        {
            // Arrange
            var newTask = new TaskItem(
                title: "Title",
                description: "Description",
                dueDate: DateTime.UtcNow.AddDays(2));

            // Act
            newTask.StartTask();
            newTask.CompleteTask();
            var act = () => newTask.CancelTask();

            // Asserts
            var exception = Assert.Throws<InvalidOperationException>(() => act());
            Assert.Equal("Completed task cannot be cancelled", exception.Message);
        }
        #endregion
    }
}
