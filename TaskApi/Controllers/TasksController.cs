using Microsoft.AspNetCore.Mvc;
using TaskApi.Models;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private static readonly List<TaskItem> _tasks =
    [
        new TaskItem
        {
            Id = 1,
            Title = "Изучить REST",
            Description = "Понять основные принципы REST API",
            IsCompleted = true,
            CreatedAt = DateTime.Now.AddDays(-3),
            Priority = "High"
        },
        new TaskItem
        {
            Id = 2,
            Title = "Создать контроллер",
            Description = "Реализовать CRUD-маршруты",
            IsCompleted = false,
            CreatedAt = DateTime.Now.AddDays(-2),
            Priority = "Normal"
        },
        new TaskItem
        {
            Id = 3,
            Title = "Протестировать API",
            Description = "Проверить маршруты через Swagger UI",
            IsCompleted = false,
            CreatedAt = DateTime.Now.AddDays(-1),
            Priority = "Low"
        }
    ];

    private static int _nextId = 4;

    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetAll([FromQuery] bool? completed)
    {
        IEnumerable<TaskItem> result = _tasks.AsEnumerable();

        if (completed.HasValue)
        {
            result = result.Where(task => task.IsCompleted == completed.Value);
        }

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<TaskItem> GetById(int id)
    {
        var task = _tasks.FirstOrDefault(task => task.Id == id);

        if (task is null)
        {
            return NotFound($"Задача с id = {id} не найдена");
        }

        return Ok(task);
    }

    [HttpPost]
    public ActionResult<TaskItem> Create([FromBody] CreateTaskDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest("Название задачи обязательно");
        }

        var task = new TaskItem
        {
            Id = _nextId++,
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = false,
            CreatedAt = DateTime.Now,
            Priority = dto.Priority
        };

        _tasks.Add(task);

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id:int}")]
    public ActionResult<TaskItem> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var task = _tasks.FirstOrDefault(task => task.Id == id);

        if (task is null)
        {
            return NotFound($"Задача с id = {id} не найдена");
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            return BadRequest("Название задачи обязательно");
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.Priority = dto.Priority;

        return Ok(task);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var task = _tasks.FirstOrDefault(task => task.Id == id);

        if (task is null)
        {
            return NotFound($"Задача с id = {id} не найдена");
        }

        _tasks.Remove(task);

        return NoContent();
    }

    [HttpPatch("{id:int}/toggle")]
    public ActionResult<TaskItem> ToggleCompleted(int id)
    {
        var task = _tasks.FirstOrDefault(task => task.Id == id);

        if (task is null)
        {
            return NotFound($"Задача с id = {id} не найдена");
        }

        task.IsCompleted = !task.IsCompleted;

        return Ok(task);
    }
}
