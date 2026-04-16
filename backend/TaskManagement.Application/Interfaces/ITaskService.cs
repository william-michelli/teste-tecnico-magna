using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<IEnumerable<TaskDto>> GetTasksByStatusAsync(string status);
        Task<IEnumerable<TaskDto>> SearchTasksAsync(string searchTerm);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
        Task DeleteTaskAsync(int id);
    }
}