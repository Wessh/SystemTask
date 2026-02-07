using Application.Dtos;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using SystemTask.Api;

namespace Api.Tests
{
    public class TaskItemIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TaskItemIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        #region GetById
        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTaskDoesNotExists()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            // Act
            var response = await _client.GetAsync($"/api/TaskItem/{nonExistentId}");
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ReturnTaskItemDto_WhenValidId()
        {
            // Arrange
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };

            var addDto = new CreateTaskItemDto
            {
                Title = "Tarefa de Teste",
                Description = "Descrição da tarefa",
                DueDate = DateTime.UtcNow.AddDays(1)
            };
            var addResponse = await _client.PostAsJsonAsync("/api/taskitem/create", addDto);
            addResponse.EnsureSuccessStatusCode();

            var createdItem = await addResponse.Content.ReadFromJsonAsync<TaskItemDto>(options); // Captura o objeto retornado pelo POST

            // Act
            var response = await _client.GetAsync($"/api/TaskItem/{createdItem!.Id}");
            response.EnsureSuccessStatusCode();
            var fetchedItem = await response.Content.ReadFromJsonAsync<TaskItemDto>(options);


            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(fetchedItem);
            Assert.Equal(createdItem.Id, fetchedItem.Id);
            Assert.Equal(createdItem.Title, fetchedItem.Title);
            Assert.Equal(createdItem.Description, fetchedItem.Description);
            Assert.Equal(createdItem.Status, fetchedItem.Status);
        }
        #endregion
        
        #region Create
        [Fact]
        public async Task Create_ReturnsCreated_WhenValidData()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };
            var dto = new { Title = "Nova tarefa", Description = "Descrição da tarefa", DueDate = DateTime.UtcNow.AddDays(1) };

            var response = await _client.PostAsJsonAsync("/api/taskitem/create", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<TaskItemDto>(options);

            Assert.Equal("Nova tarefa", created!.Title);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenInvalidData()
        {
            var dto = new { Title = "", Description = "Descrição da tarefa", DueDate = DateTime.UtcNow.AddDays(1) };
            var response = await _client.PostAsJsonAsync("/api/taskitem/create", dto);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
        #endregion
        
        #region Start
        [Fact]
        public async Task Start_ReturnsNotFound_WhenIdIsEmpty()
        {
            var response = await _client.PutAsync("/api/taskitem//start", null);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Start_ReturnsTaskItemDto_WhenValidId()
        {
            // Arrange
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };
            var addDto = new CreateTaskItemDto
            {
                Title = "Tarefa de Teste",
                Description = "Descrição da tarefa",
                DueDate = DateTime.UtcNow.AddDays(1)
            };
            var addResponse = await _client.PostAsJsonAsync("/api/taskitem/create", addDto);
            addResponse.EnsureSuccessStatusCode();
            var createdItem = await addResponse.Content.ReadFromJsonAsync<TaskItemDto>(options); // Captura o objeto retornado pelo POST
            // Act
            var response = await _client.PutAsync($"/api/TaskItem/{createdItem!.Id}/start", null);
            response.EnsureSuccessStatusCode();
            var startedItem = await response.Content.ReadFromJsonAsync<TaskItemDto>(options);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(startedItem);
            Assert.Equal(createdItem.Id, startedItem.Id);
            Assert.Equal(StatusTask.InProgress, startedItem.Status);
        }

        #endregion

        #region OnHold
        [Fact]
        public async Task OnHold_ReturnsNotFound_WhenIdIsEmpty()
        {
            var response = await _client.PutAsync("/api/taskitem//on-hold", null);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task OnHold_ReturnsTaskItemDto_WhenValidId()
        {
            // Arrange
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };
            var addDto = new CreateTaskItemDto
            {
                Title = "Tarefa de Teste",
                Description = "Descrição da tarefa",
                DueDate = DateTime.UtcNow.AddDays(1)
            };
            var addResponse = await _client.PostAsJsonAsync("/api/taskitem/create", addDto);
            addResponse.EnsureSuccessStatusCode();
            var createdItem = await addResponse.Content.ReadFromJsonAsync<TaskItemDto>(options); // Captura o objeto retornado pelo POST
            var moveOnHold = await _client.PutAsync($"/api/TaskItem/{createdItem!.Id}/start", null);
            moveOnHold.EnsureSuccessStatusCode();

            // Act
            var response = await _client.PutAsync($"/api/TaskItem/{createdItem!.Id}/on-hold", null);
            response.EnsureSuccessStatusCode();
            var onHoldItem = await response.Content.ReadFromJsonAsync<TaskItemDto>(options);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(onHoldItem);
            Assert.Equal(createdItem.Id, onHoldItem.Id);
            Assert.Equal(StatusTask.OnHold, onHoldItem.Status);
        }
        #endregion

        #region Complete
        [Fact]
        public async Task Complete_ReturnsNotFound_WhenIdIsEmpty()
        {
            var response = await _client.PutAsync("/api/taskitem//complete", null);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Complete_ReturnsTaskItemDto_WhenValidId()
        {
            // Arrange
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };
            var addDto = new CreateTaskItemDto
            {
                Title = "Tarefa de Teste",
                Description = "Descrição da tarefa",
                DueDate = DateTime.UtcNow.AddDays(1)
            };
            var addResponse = await _client.PostAsJsonAsync("/api/taskitem/create", addDto);
            addResponse.EnsureSuccessStatusCode();
            var createdItem = await addResponse.Content.ReadFromJsonAsync<TaskItemDto>(options); // Captura o objeto retornado pelo POST
            var moveOnHold = await _client.PutAsync($"/api/TaskItem/{createdItem!.Id}/start", null);
            moveOnHold.EnsureSuccessStatusCode();
            // Act
            var response = await _client.PutAsync($"/api/TaskItem/{createdItem!.Id}/complete", null);
            response.EnsureSuccessStatusCode();
            var completedItem = await response.Content.ReadFromJsonAsync<TaskItemDto>(options);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(completedItem);
            Assert.Equal(createdItem.Id, completedItem.Id);
            Assert.Equal(StatusTask.Completed, completedItem.Status);
        }
        #endregion

        #region Cancel
        [Fact]
        public async Task Cancel_ReturnsNotFound_WhenIdIsEmpty()
        {
            var response = await _client.PutAsync("/api/taskitem//cancel", null);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Cancel_ReturnsTaskItemDto_WhenValidId()
        {
            // Arrange
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } };
            var addDto = new CreateTaskItemDto
            {
                Title = "Tarefa de Teste",
                Description = "Descrição da tarefa",
                DueDate = DateTime.UtcNow.AddDays(1)
            };
            var addResponse = await _client.PostAsJsonAsync("/api/taskitem/create", addDto);
            addResponse.EnsureSuccessStatusCode();
            var createdItem = await addResponse.Content.ReadFromJsonAsync<TaskItemDto>(options); // Captura o objeto retornado pelo POST
            var cancelResponse = await _client.PutAsync($"/api/TaskItem/{createdItem!.Id}/cancel", null);
            cancelResponse.EnsureSuccessStatusCode();
            // Act
            var response = await _client.PutAsync($"/api/TaskItem/{createdItem!.Id}/cancel", null);
            response.EnsureSuccessStatusCode();
            var canceledItem = await response.Content.ReadFromJsonAsync<TaskItemDto>(options);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(canceledItem);
            Assert.Equal(createdItem.Id, canceledItem.Id);
            Assert.Equal(StatusTask.Cancelled, canceledItem.Status);
        }
        #endregion

    }
}
