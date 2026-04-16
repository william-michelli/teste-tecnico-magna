using Moq;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using DomainTaskStatus = TaskManagement.Domain.Entities.TaskStatus;

namespace TaskManagement.Application.Tests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepository;
    private readonly ITaskService _taskService;

    public TaskServiceTests()
    {
        _mockRepository = new Mock<ITaskRepository>();
        _taskService = new TaskService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var task = new TaskEntity { Id = 1, Title = "Test Task", Status = DomainTaskStatus.Pendente };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

        // Act
        var result = await _taskService.GetTaskByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.GetTaskByIdAsync(1));
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            new() { Id = 1, Title = "Task 1", Status = DomainTaskStatus.Pendente },
            new() { Id = 2, Title = "Task 2", Status = DomainTaskStatus.Concluido }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);

        // Act
        var result = await _taskService.GetAllTasksAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Task 1", result.First().Title);
    }

    [Fact]
    public async Task GetTasksByStatusAsync_ShouldReturnFilteredTasks()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            new() { Id = 1, Title = "Task 1", Status = DomainTaskStatus.Pendente }
        };
        _mockRepository.Setup(r => r.GetByStatusAsync(DomainTaskStatus.Pendente)).ReturnsAsync(tasks);

        // Act
        var result = await _taskService.GetTasksByStatusAsync("Pendente");

        // Assert
        Assert.Single(result);
        Assert.Equal("Task 1", result.First().Title);
    }

    [Fact]
    public async Task GetTasksByStatusAsync_ShouldThrowArgumentException_WhenInvalidStatus()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _taskService.GetTasksByStatusAsync("InvalidStatus"));
    }

    [Fact]
    public async Task SearchTasksAsync_ShouldReturnMatchingTasks()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            new() { Id = 1, Title = "Important Task", Status = DomainTaskStatus.Pendente }
        };
        _mockRepository.Setup(r => r.SearchAsync("Important")).ReturnsAsync(tasks);

        // Act
        var result = await _taskService.SearchTasksAsync("Important");

        // Assert
        Assert.Single(result);
        Assert.Equal("Important Task", result.First().Title);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateTask_WhenValidData()
    {
        // Arrange
        var createDto = new CreateTaskDto { Title = "New Task", Description = "Description", Status = DomainTaskStatus.Pendente };
        var createdTask = new TaskEntity { Id = 1, Title = "New Task", Description = "Description", Status = DomainTaskStatus.Pendente, CreatedAt = DateTime.UtcNow };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TaskEntity>())).Callback<TaskEntity>(t => t.Id = 1);

        // Act
        var result = await _taskService.CreateTaskAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Task", result.Title);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TaskEntity>()), Times.Once);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldThrowArgumentException_WhenTitleIsEmpty()
    {
        // Arrange
        var createDto = new CreateTaskDto { Title = "", Status = DomainTaskStatus.Pendente };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(createDto));
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldThrowArgumentException_WhenCompletingTaskWithoutTitle()
    {
        // Arrange
        var createDto = new CreateTaskDto { Title = "", Status = DomainTaskStatus.Concluido };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(createDto));
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask_WhenValidData()
    {
        // Arrange
        var existingTask = new TaskEntity { Id = 1, Title = "Old Title", Status = DomainTaskStatus.Pendente };
        var updateDto = new UpdateTaskDto { Title = "New Title", Description = "New Description", Status = DomainTaskStatus.EmAndamento };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingTask);

        // Act
        await _taskService.UpdateTaskAsync(1, updateDto);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<TaskEntity>(t =>
            t.Id == 1 &&
            t.Title == "New Title" &&
            t.Description == "New Description" &&
            t.Status == DomainTaskStatus.EmAndamento)), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.UpdateTaskAsync(1, new UpdateTaskDto { Title = "Title" }));
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldDeleteTask_WhenTaskIsNotCompleted()
    {
        // Arrange
        var task = new TaskEntity { Id = 1, Title = "Task", Status = DomainTaskStatus.Pendente };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

        // Act
        await _taskService.DeleteTaskAsync(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldThrowInvalidOperationException_WhenTaskIsCompleted()
    {
        // Arrange
        var task = new TaskEntity { Id = 1, Title = "Task", Status = DomainTaskStatus.Concluido };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.DeleteTaskAsync(1));
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.DeleteTaskAsync(1));
    }
}
