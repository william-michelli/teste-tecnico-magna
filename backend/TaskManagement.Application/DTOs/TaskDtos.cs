using TaskManagement.Domain.Entities;
using DomainTaskStatus = TaskManagement.Domain.Entities.TaskStatus;

namespace TaskManagement.Application.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DomainTaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DomainTaskStatus Status { get; set; } = DomainTaskStatus.Pendente;
    }

    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DomainTaskStatus Status { get; set; }
    }
}