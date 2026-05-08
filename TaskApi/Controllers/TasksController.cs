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

    [HttpGet("search")]
    public ActionResult<IEnumerable<TaskItem>> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Поисковая строка обязательна");
        }

        var result = _tasks.Where(task =>
            task.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
            task.Description.Contains(query, StringComparison.OrdinalIgnoreCase));

        return Ok(result);
    }

    [HttpGet("priority/{level}")]
    public ActionResult<IEnumerable<TaskItem>> GetByPriority(string level)
    {
        var result = _tasks.Where(task =>
            task.Priority.Equals(level, StringComparison.OrdinalIgnoreCase));

        return Ok(result);
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var stats = new
        {
            total = _tasks.Count,
            completed = _tasks.Count(task => task.IsCompleted),
            notCompleted = _tasks.Count(task => !task.IsCompleted),
            highPriority = _tasks.Count(task => task.Priority == "High")
        };

        return Ok(stats);
    }

    [HttpGet("sorted")]
    public ActionResult<IEnumerable<TaskItem>> GetSorted([FromQuery] string? by)
    {
        var result = by?.ToLower() switch
        {
            "title" => _tasks.OrderBy(task => task.Title),
            "priority" => _tasks.OrderBy(task => task.Priority),
            "createdat" => _tasks.OrderBy(task => task.CreatedAt),
            _ => _tasks.OrderBy(task => task.Id)
        };

        return Ok(result);
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
