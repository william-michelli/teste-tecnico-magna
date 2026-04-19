using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
    {
        try
        {
            if (!string.IsNullOrEmpty(status))
            {
                var tasks = await _taskService.GetTasksByStatusAsync(status, page, pageSize);
                return Ok(tasks);
            }
            if (!string.IsNullOrEmpty(search))
            {
                var tasks = await _taskService.SearchTasksAsync(search, page, pageSize);
                return Ok(tasks);
            }
            var allTasks = await _taskService.GetAllTasksAsync(page, pageSize);
            return Ok(allTasks);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(task);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
     
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            var task = await _taskService.CreateTaskAsync(createTaskDto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            await _taskService.UpdateTaskAsync(id, updateTaskDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("concluir/{id}")]
    public async Task<IActionResult> Conclude(int id, [FromBody] ConcludeTaskDto updateTaskDto)
    {
        try
        {
            await _taskService.ConcludeTaskAsync(id, updateTaskDto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}