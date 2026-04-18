using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using DomainTaskStatus = TaskManagement.Domain.Entities.TaskStatus;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

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

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(MapToDto);
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByStatusAsync(string status)
        {
            if (!Enum.TryParse<DomainTaskStatus>(status, true, out var taskStatus))
                throw new ArgumentException("Status inválido");

            var tasks = await _taskRepository.GetByStatusAsync(taskStatus);
            return tasks.Select(MapToDto);
        }

        public async Task<IEnumerable<TaskDto>> SearchTasksAsync(string searchTerm)
        {
            var tasks = await _taskRepository.SearchAsync(searchTerm);
            return tasks.Select(MapToDto);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            if (string.IsNullOrWhiteSpace(createTaskDto.Title))
                throw new ArgumentException("Título é obrigatório");

            if (createTaskDto.Status == DomainTaskStatus.Concluido && string.IsNullOrWhiteSpace(createTaskDto.Title))
                throw new ArgumentException("Não é possível criar uma tarefa sem um título");

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

            if (updateTaskDto.Status == DomainTaskStatus.Concluido && string.IsNullOrWhiteSpace(updateTaskDto.Title))
                throw new ArgumentException("ão é possível atualizar uma tarefa sem um título");

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
}