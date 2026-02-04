using Application.Dtos;
using Application.Helper;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Moq;
using System.Threading.Tasks;

namespace Application.Tests
{
    public class TaskItemServiceTests
    {
        private readonly Mock<ITaskItemRepository> _repositoryMock;
        private readonly ITaskItemService _service;

        public TaskItemServiceTests()
        {
            _repositoryMock = new Mock<ITaskItemRepository>();
            _service = new TaskItemService(_repositoryMock.Object);
        }

        #region AddAsync
        [Fact]
        public async Task AddAsync_ShouldThrow_WhenTitleIsEmpty()
        {
            // Arrange
            var taskItem = new CreateTaskItemDto("", "Description", DateTime.UtcNow.AddDays(1));

            // Act
            var taskException = () => _service.AddAsync(taskItem);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(taskException);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenDueDateIsPast()
        {
            // Arrange
            var taskItem = new CreateTaskItemDto("Title", "Description", DateTime.UtcNow.AddDays(-1));

            // Act
            var taskException = () => _service.AddAsync(taskItem);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(taskException);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnDto_WhenValid()
        {
            // Arrange
            var dto = new CreateTaskItemDto("Title", "Description", DateTime.UtcNow.AddDays(1));

            // Act
            var result = await _service.AddAsync(dto);

            // Assert
            Assert.Equal(dto.Title, result.Title);
            Assert.Equal(dto.Description, result.Description);
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenIdIsEmpty()
        {
            // Arrange
            // Act
            var taskException = () => _service.GetByIdAsync(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(taskException);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrow_WhenTaskNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenTaskExists()
        {
            // Arrange
            var taskItem = new TaskItem("Title", "Description", DateTime.UtcNow.AddDays(1));
            _repositoryMock.Setup(r => r.GetByIdAsync(taskItem.Id)).ReturnsAsync(taskItem);

            // Act
            var result = await _service.GetByIdAsync(taskItem.Id);

            // Assert
            Assert.Equal(taskItem.Title, result.Title);

        }
        #endregion

        #region StartAsync
        [Fact]
        public async Task StartAsync_ShouldThrow_WhenIdIsEmpty()
        {
            //Act
            var taskException = () => _service.StartAsync(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(taskException);
        }

        [Fact]
        public async Task StartAsync_ShouldThrow_WhenTaskNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.StartAsync(id));
        }

        [Fact]
        public async Task StartAsync_ShouldUpdateTask_WhenValid()
        {
            // Arrange
            var taskItem = new TaskItem("Title", "Description", DateTime.UtcNow.AddDays(1));
            _repositoryMock.Setup(r => r.GetByIdAsync(taskItem.Id)).ReturnsAsync(taskItem);

            // Act
            var result = await _service.StartAsync(taskItem.Id);

            // Assert
            Assert.Equal(StatusTask.InProgress, result.Status);
            _repositoryMock.Verify(r => r.UpdateAsync(taskItem), Times.Once);
        }
        #endregion

        #region OnHoldAsync
        [Fact]
        public async Task OnHoldAsync_ShouldThrow_WhenIdIsEmpty()
        {
            //Act
            var taskException = () => _service.OnHoldAsync(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(taskException);
        }

        [Fact]
        public async Task OnHoldAsync_ShouldThrow_WhenTaskNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.OnHoldAsync(id));
        }

        [Fact]
        public async Task OnHoldAsync_ShouldUpdateStatus_WhenValid()
        {
            // Arrange
            var taskItem = new TaskItem("Title", "Description", DateTime.UtcNow.AddDays(1));
            _repositoryMock.Setup(r => r.GetByIdAsync(taskItem.Id)).ReturnsAsync(taskItem);

            // Act
            await _service.StartAsync(taskItem.Id);
            var result = await _service.OnHoldAsync(taskItem.Id);

            // Assert
            Assert.Equal(StatusTask.OnHold, result.Status);
            _repositoryMock.Verify(r => r.UpdateAsync(taskItem), Times.Exactly(2));
        }
        #endregion

        #region CompleteAsync
        [Fact]
        public async Task CompleteAsync_ShouldThrow_WhenIdIsEmpty()
        {
            //Act
            var taskException = () => _service.CompleteAsync(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(taskException);
        }

        [Fact]
        public async Task CompleteAsync_ShouldThrow_WhenTaskNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CompleteAsync(id));
        }

        [Fact]
        public async Task CompleteAsync_ShouldUpdateStatus_WhenValid()
        {
            // Arrange
            var taskItem = new TaskItem("Title", "Description", DateTime.UtcNow.AddDays(1));
            _repositoryMock.Setup(r => r.GetByIdAsync(taskItem.Id)).ReturnsAsync(taskItem);

            // Act
            await _service.StartAsync(taskItem.Id);
            var result = await _service.CompleteAsync(taskItem.Id);

            // Assert
            Assert.Equal(StatusTask.Completed, result.Status);
            _repositoryMock.Verify(r => r.UpdateAsync(taskItem), Times.Exactly(2));
        }
        #endregion

        #region CancelAsync
        [Fact]
        public async Task CancelAsync_ShouldThrow_WhenIdIsEmpty()
        {
            //Act
            var taskException = () => _service.CancelAsync(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<ArgumentException>(taskException);
        }

        [Fact]
        public async Task CancelAsync_ShouldThrow_WhenTaskNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskItem?)null);

            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CancelAsync(id));
        }

        [Fact]
        public async Task CancelAsync_ShouldUpdateStatus_WhenValid()
        {
            // Arrange
            var taskItem = new TaskItem("Title", "Description", DateTime.UtcNow.AddDays(1));
            _repositoryMock.Setup(r => r.GetByIdAsync(taskItem.Id)).ReturnsAsync(taskItem);

            // Act
            var result = await _service.CancelAsync(taskItem.Id);

            // Assert
            Assert.Equal(StatusTask.Cancelled, result.Status);
            _repositoryMock.Verify(r => r.UpdateAsync(taskItem), Times.Once);
        }
        #endregion
    }
}
