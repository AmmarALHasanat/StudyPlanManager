using backend.Data;
using backend.DTO;
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
        public IActionResult AddTask(int weekId, [FromBody] TaskDto dto)
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
                Industry = dto.Industry ?? string.Empty,
                ActivityType = dto.ActivityType ?? "Reading",
                ActivityName = dto.ActivityName ?? string.Empty,
                Target = dto.Target ?? string.Empty,
                Notes = dto.Notes ?? string.Empty,
                Status = dto.Status ?? "Pending"
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return Ok(task);
        }

        // PUT: api/task/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskDto dto)
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
            task.Industry = dto.Industry ?? task.Industry;
            task.ActivityType = dto.ActivityType ?? task.ActivityType;
            task.ActivityName = dto.ActivityName ?? task.ActivityName;
            task.Target = dto.Target ?? task.Target;
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

}
