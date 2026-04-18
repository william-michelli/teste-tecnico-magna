using Moq;
using TaskManagement.Application.Common;
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
    private readonly int page = 1;
    private readonly int pageSize = 100;

    private readonly PaginatedResult<TaskEntity> pagedResult = new() { 
        Items = new List<TaskEntity> {
            new () { Id = 1, Title = "Task 1", Status = DomainTaskStatus.Pendente},
            new () { Id = 2, Title = "Task 2", Status = DomainTaskStatus.Concluido },
            new () { Id = 3, Title = "Important Task", Status = DomainTaskStatus.Pendente }
        },
        TotalCount = 2,
        Page = 1,
        PageSize = 100
    };

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
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.GetTaskByIdAsync(1));
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        _mockRepository.Setup(r => r.GetAllAsync(page, pageSize)).ReturnsAsync(pagedResult);

        // Act
        var result = await _taskService.GetAllTasksAsync(page, pageSize);

        // Assert
        Assert.Equal(2, result.Items.Count());
        Assert.Equal("Task 1", result.Items.First().Title);
    }

    [Fact]
    public async Task GetTasksByStatusAsync_ShouldReturnFilteredTasks()
    {
        _mockRepository.Setup(r => r.GetByStatusAsync(DomainTaskStatus.Pendente, page, pageSize)).ReturnsAsync(pagedResult);

        var result = await _taskService.GetTasksByStatusAsync("Pendente", page, pageSize);

        Assert.Single(result.Items);
        Assert.Equal("Task 1", result.Items.First().Title);
    }

    [Fact]
    public async Task GetTasksByStatusAsync_ShouldThrowArgumentException_WhenInvalidStatus()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _taskService.GetTasksByStatusAsync("InvalidStatus", page, pageSize));
    }

    [Fact]
    public async Task SearchTasksAsync_ShouldReturnMatchingTasks()
    {
        _mockRepository.Setup(r => r.SearchAsync("Important", page, pageSize)).ReturnsAsync(pagedResult);

        var result = await _taskService.SearchTasksAsync("Important", page, pageSize);

        Assert.Single(result.Items);
        Assert.Equal("Important Task", result.Items.First().Title);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateTask_WhenValidData()
    {
        var createDto = new CreateTaskDto { Title = "New Task", Description = "Description", Status = DomainTaskStatus.Pendente };
        var createdTask = new TaskEntity { Id = 1, Title = "New Task", Description = "Description", Status = DomainTaskStatus.Pendente, CreatedAt = DateTime.UtcNow };

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TaskEntity>())).Callback<TaskEntity>(t => t.Id = 1);

        var result = await _taskService.CreateTaskAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal("New Task", result.Title);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<TaskEntity>()), Times.Once);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldThrowArgumentException_WhenTitleIsEmpty()
    {
        var createDto = new CreateTaskDto { Title = "", Status = DomainTaskStatus.Pendente };

        await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(createDto));
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldThrowArgumentException_WhenCompletingTaskWithoutTitle()
    {
        var createDto = new CreateTaskDto { Title = "", Status = DomainTaskStatus.Concluido };

        await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(createDto));
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask_WhenValidData()
    {
        var existingTask = new TaskEntity { Id = 1, Title = "Old Title", Status = DomainTaskStatus.Pendente };
        var updateDto = new UpdateTaskDto { Title = "New Title", Description = "New Description", Status = DomainTaskStatus.EmAndamento };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingTask);

        await _taskService.UpdateTaskAsync(1, updateDto);

        _mockRepository.Verify(r => r.UpdateAsync(It.Is<TaskEntity>(t =>
            t.Id == 1 &&
            t.Title == "New Title" &&
            t.Description == "New Description" &&
            t.Status == DomainTaskStatus.EmAndamento)), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskEntity?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.UpdateTaskAsync(1, new UpdateTaskDto { Title = "Title" }));
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldDeleteTask_WhenTaskIsNotCompleted()
    {
        var task = new TaskEntity { Id = 1, Title = "Task", Status = DomainTaskStatus.Pendente };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

        await _taskService.DeleteTaskAsync(1);

        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldThrowInvalidOperationException_WhenTaskIsCompleted()
    {
        var task = new TaskEntity { Id = 1, Title = "Task", Status = DomainTaskStatus.Concluido };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.DeleteTaskAsync(1));
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskEntity?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.DeleteTaskAsync(1));
    }
}
