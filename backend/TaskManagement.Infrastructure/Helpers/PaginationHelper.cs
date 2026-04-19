using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Common;

namespace TaskManagement.Infrastructure.Helpers;

public static class PaginationHelper
{
    public static async Task<PaginatedResult<T>> PaginateAsync<T>(IQueryable<T> query, int page, int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
    }
}
