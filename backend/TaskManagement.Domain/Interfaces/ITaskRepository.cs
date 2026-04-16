using TaskManagement.Domain.Entities;
using DomainTaskStatus = TaskManagement.Domain.Entities.TaskStatus;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TaskEntity>> GetAllAsync();
        Task<IEnumerable<TaskEntity>> GetByStatusAsync(DomainTaskStatus status);
        Task<IEnumerable<TaskEntity>> SearchAsync(string searchTerm);
        Task AddAsync(TaskEntity task);
        Task UpdateAsync(TaskEntity task);
        Task DeleteAsync(int id);
    }
}