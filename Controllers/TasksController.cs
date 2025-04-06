using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

[Route("tasks")]
[ApiController]
public class TasksController : ControllerBase
{
    private const string FilePath = "tasks.json";

    [HttpGet]
    public ActionResult<IEnumerable<Task>> GetTasks()
    {
        var tasks = JsonSerializer.Deserialize<List<Task>>(System.IO.File.ReadAllText(FilePath)) ?? new List<Task>();
        return Ok(tasks);
    }

    [HttpPost]
    public ActionResult<Task> AddTask([FromBody] Task task)
    {
        var tasks = JsonSerializer.Deserialize<List<Task>>(System.IO.File.ReadAllText(FilePath)) ?? new List<Task>();
        
        tasks.Add(task);
        System.IO.File.WriteAllText(FilePath, JsonSerializer.Serialize(tasks));
        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateTask(int id, [FromBody] Task updatedTask)
    {
        var tasks = JsonSerializer.Deserialize<List<Task>>(System.IO.File.ReadAllText(FilePath)) ?? new List<Task>();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound();

        task.Title = updatedTask.Title;
        task.Description = updatedTask.Description;
        task.Priority = updatedTask.Priority;
        task.DueDate = updatedTask.DueDate;
        task.Status = updatedTask.Status;

        System.IO.File.WriteAllText(FilePath, JsonSerializer.Serialize(tasks));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteTask(int id)
    {
        var tasks = JsonSerializer.Deserialize<List<Task>>(System.IO.File.ReadAllText(FilePath)) ?? new List<Task>();
        var task = tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return NotFound();

        tasks.Remove(task);
        System.IO.File.WriteAllText(FilePath, JsonSerializer.Serialize(tasks));
        return NoContent();
    }
}
