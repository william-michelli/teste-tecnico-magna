using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using DomainTaskStatus = TaskManagement.Domain.Entities.TaskStatus;


namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagementDbContext _context;

    public TaskRepository(TaskManagementDbContext context)
    {
        _context = context;
    }

    public async Task<TaskEntity?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<PaginatedResult<TaskEntity>> GetAllAsync(int page, int pageSize)
    {
        var query = _context.Tasks
            .OrderByDescending(t => t.CreatedAt);

        return await PaginationHelper.PaginateAsync(query, page, pageSize);
    }

    public async Task<PaginatedResult<TaskEntity>> GetByStatusAsync(DomainTaskStatus status, int page, int pageSize)
    {
        var query = _context.Tasks
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedAt);

        return await PaginationHelper.PaginateAsync(query, page, pageSize);
    }

    public async Task<PaginatedResult<TaskEntity>> SearchAsync(string searchTerm, int page, int pageSize)
    {
        var query = _context.Tasks
            .Where(t => t.Title.Contains(searchTerm) || (t.Description != null && t.Description.Contains(searchTerm)))
            .OrderByDescending(t => t.CreatedAt);

        return await PaginationHelper.PaginateAsync(query, page, pageSize);
    }

    public async Task AddAsync(TaskEntity task)
    {
        try {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }catch(Exception ex)
        {
            throw;
        }
    }

    public async Task UpdateAsync(TaskEntity task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}