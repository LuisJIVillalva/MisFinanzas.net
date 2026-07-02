using CursoApis.Models;
using CursoApis.Services;
using Microsoft.AspNetCore.Mvc;

namespace CursoApis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskServices _taskService;

    public TaskController(ITaskServices taskService)
    {
        _taskService = taskService;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskItem task)
    {
        var createdTask = await _taskService.CreateAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _taskService.DeleteByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskItem pTask)
    {
        var updateTask = await _taskService.UpdateByIdAsync(id, pTask);
        if (updateTask == null) return NotFound();
        return Ok(updateTask);
    }
}