using System.Threading.Tasks;
using TaskManagement.Application.Common;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto> GetTaskByIdAsync(int id);
    Task<PaginatedResult<TaskDto>> GetAllTasksAsync(int page, int pageSize);
    Task<PaginatedResult<TaskDto>> GetTasksByStatusAsync(string status, int page, int pageSize);
    Task<PaginatedResult<TaskDto>> SearchTasksAsync(string searchTerm, int page, int pageSize);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
    Task UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
    Task ConcludeTaskAsync(int id, ConcludeTaskDto updateTaskDto);
    Task DeleteTaskAsync(int id);
}