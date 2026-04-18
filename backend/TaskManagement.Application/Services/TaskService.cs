using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using DomainTaskStatus = TaskManagement.Domain.Entities.TaskStatus;

namespace TaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly int titleCharacterLimit = 50;
    private readonly int descriptionCharacterLimit = 100;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) throw new KeyNotFoundException("Tarefa não encontrada");

        return MapToDto(task);
    }

    public async Task<PaginatedResult<TaskDto>> GetAllTasksAsync(int page, int pageSize)
    {
        var result = await _taskRepository.GetAllAsync(page, pageSize);
        return new PaginatedResult<TaskDto>
        {
            Items = result.Items.Select(MapToDto),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<PaginatedResult<TaskDto>> GetTasksByStatusAsync(string status, int page, int pageSize)
    {
        if (!Enum.TryParse<DomainTaskStatus>(status, true, out var taskStatus))
            throw new ArgumentException("Status inválido");

        var result = await _taskRepository.GetByStatusAsync(taskStatus, page, pageSize);
        return new PaginatedResult<TaskDto>
        {
            Items = result.Items.Select(MapToDto),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<PaginatedResult<TaskDto>> SearchTasksAsync(string searchTerm, int page, int pageSize)
    {
        var result = await _taskRepository.SearchAsync(searchTerm, page, pageSize);
        return new PaginatedResult<TaskDto>
        {
            Items = result.Items.Select(MapToDto),
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
    {
        if (string.IsNullOrWhiteSpace(createTaskDto.Title))
            throw new ArgumentException("Título é obrigatório");

        if (createTaskDto.Title.Length > titleCharacterLimit)
            throw new ArgumentException($"O título ultrapassa o limite de {titleCharacterLimit} caracteres. Por favor, reduza o conteúdo.");

        if (createTaskDto.Description != null && createTaskDto.Description.Length > descriptionCharacterLimit)
            throw new ArgumentException($"A descrição ultrapassa o limite de {descriptionCharacterLimit} caracteres. Por favor, reduza o conteúdo.");

        var task = new TaskEntity
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            Status = createTaskDto.Status,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddAsync(task);
        return MapToDto(task);
    }

    public async Task UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) throw new KeyNotFoundException("Tarefa não encontrada");

        if (string.IsNullOrWhiteSpace(updateTaskDto.Title))
            throw new ArgumentException("Título é obrigatório");

        if (updateTaskDto.Title.Length > titleCharacterLimit)
            throw new ArgumentException($"O título ultrapassa o limite de {titleCharacterLimit} caracteres. Por favor, reduza o conteúdo.");

        if (updateTaskDto.Description != null && updateTaskDto.Description.Length > descriptionCharacterLimit)
            throw new ArgumentException($"A descrição ultrapassa o limite de {descriptionCharacterLimit} caracteres. Por favor, reduza o conteúdo.");

        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        task.Status = updateTaskDto.Status;
        task.EditedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);
    }

    public async Task ConcludeTaskAsync(int id, ConcludeTaskDto updateTaskDto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) throw new KeyNotFoundException("Tarefa não encontrada");

        task.Status = updateTaskDto.Status;

        await _taskRepository.UpdateAsync(task);
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) throw new KeyNotFoundException("Tarefa não encontrada");

        if (task.Status == DomainTaskStatus.Concluido)
            throw new InvalidOperationException("Tarefa já concluída não pode ser deletada");

        await _taskRepository.DeleteAsync(id);
    }

    private static TaskDto MapToDto(TaskEntity task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            EditedAt = task.EditedAt
        };
    }
}