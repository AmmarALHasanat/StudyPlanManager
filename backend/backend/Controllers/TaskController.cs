using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/task/week/{weekId}
        [HttpGet("week/{weekId:int}")]
        public IActionResult GetTasksByWeek(int weekId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var week = _context.Weeks.Include(w => w.Plan).FirstOrDefault(w => w.Id == weekId && w.Plan.UserId == user.Id);
            if (week == null) return NotFound("Week not found or access denied");

            var tasks = _context.Tasks.Where(t => t.WeekId == weekId).OrderBy(t => t.Id).ToList();
            return Ok(tasks);
        }

        // POST: api/task/{weekId}
        [HttpPost("{weekId:int}")]
        public IActionResult AddTask(int weekId, [FromBody] CreateTaskDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var week = _context.Weeks.Include(w => w.Plan).FirstOrDefault(w => w.Id == weekId && w.Plan.UserId == user.Id);
            if (week == null) return NotFound("Week not found or access denied");

            var task = new TaskItem
            {
                WeekId = weekId,
                Day = dto.Day ?? string.Empty,
                Title = dto.Title ?? string.Empty,
                ActivityType = dto.ActivityType ?? "Reading",
                Book = dto.Book ?? string.Empty,
                Pages = dto.Pages ?? string.Empty,
                Notes = dto.Notes ?? string.Empty,
                Status = dto.Status ?? "Pending"
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return Ok(task);
        }

        // PUT: api/task/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateTask(int id, [FromBody] UpdateTaskDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var task = _context.Tasks
                .Include(t => t.Week)
                .ThenInclude(w => w.Plan)
                .FirstOrDefault(t => t.Id == id && t.Week.Plan.UserId == user.Id);

            if (task == null) return NotFound("Task not found or access denied");

            task.Day = dto.Day ?? task.Day;
            task.Title = dto.Title ?? task.Title;
            task.ActivityType = dto.ActivityType ?? task.ActivityType;
            task.Book = dto.Book ?? task.Book;
            task.Pages = dto.Pages ?? task.Pages;
            task.Notes = dto.Notes ?? task.Notes;
            if (!string.IsNullOrEmpty(dto.Status)) task.Status = dto.Status;

            _context.SaveChanges();
            return Ok(task);
        }

        // DELETE: api/task/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteTask(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var task = _context.Tasks
                .Include(t => t.Week)
                .ThenInclude(w => w.Plan)
                .FirstOrDefault(t => t.Id == id && t.Week.Plan.UserId == user.Id);

            if (task == null) return NotFound("Task not found or access denied");

            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return Ok(new { message = "Task deleted" });
        }
    }

    // DTOs
    public class CreateTaskDto
    {
        public string? Day { get; set; } // e.g., "Monday"
        public string? Title { get; set; }
        public string? ActivityType { get; set; } // Reading, Practice, Project...
        public string? Book { get; set; }
        public string? Pages { get; set; } // e.g., "pp.10-20" or "Chapter 3"
        public string? Notes { get; set; }
        public string? Status { get; set; } = "Pending";
    }

    public class UpdateTaskDto
    {
        public string? Day { get; set; }
        public string? Title { get; set; }
        public string? ActivityType { get; set; }
        public string? Book { get; set; }
        public string? Pages { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; } // Done / InProgress / Pending
    }
}
