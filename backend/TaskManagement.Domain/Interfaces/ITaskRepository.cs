using TaskManagement.Application.Common;
using TaskManagement.Domain.Entities;
using DomainTaskStatus = TaskManagement.Domain.Entities.TaskStatus;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(int id);
    Task<PaginatedResult<TaskEntity>> GetAllAsync(int page, int pageSize);
    Task<PaginatedResult<TaskEntity>> GetByStatusAsync(DomainTaskStatus status, int page, int pageSize);
    Task<PaginatedResult<TaskEntity>> SearchAsync(string searchTerm, int page, int pageSize);
    Task AddAsync(TaskEntity task);
    Task UpdateAsync(TaskEntity task);
    Task DeleteAsync(int id);
}