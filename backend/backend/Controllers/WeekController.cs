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
    public class WeekController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WeekController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/week/{planId}
        [HttpGet("{planId:int}")]
        public IActionResult GetWeeks(int planId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var plan = _context.Plans
                .Include(p => p.Weeks)
                .FirstOrDefault(p => p.Id == planId && p.UserId == user.Id);

            if (plan == null) return NotFound("Plan not found or access denied");

            return Ok(plan.Weeks.OrderBy(w => w.WeekNumber));
        }

        // POST: api/week/{planId}
        [HttpPost("{planId:int}")]
        public IActionResult AddWeek(int planId, [FromBody] CreateWeekDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var plan = _context.Plans.Include(p => p.Weeks).FirstOrDefault(p => p.Id == planId && p.UserId == user.Id);
            if (plan == null) return NotFound("Plan not found or access denied");

            int nextWeekNumber = plan.Weeks.Any() ? plan.Weeks.Max(w => w.WeekNumber) + 1 : 1;
            var week = new Week
            {
                PlanId = planId,
                WeekNumber = dto.WeekNumber > 0 ? dto.WeekNumber : nextWeekNumber,
                Summary = dto.Summary ?? string.Empty,
                Progress = dto.Progress
            };

            _context.Weeks.Add(week);
            _context.SaveChanges();

            return Ok(week);
        }

        // PUT: api/week/{id}
        [HttpPut("{id:int}")]
        public IActionResult UpdateWeek(int id, [FromBody] UpdateWeekDto dto)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var week = _context.Weeks.Include(w => w.Plan).FirstOrDefault(w => w.Id == id && w.Plan.UserId == user.Id);
            if (week == null) return NotFound("Week not found or access denied");

            week.Summary = dto.Summary ?? week.Summary;
            week.Progress = dto.Progress ?? week.Progress;
            if (dto.WeekNumber.HasValue) week.WeekNumber = dto.WeekNumber.Value;

            _context.SaveChanges();
            return Ok(week);
        }

        // DELETE: api/week/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteWeek(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized();

            var week = _context.Weeks.Include(w => w.Plan).FirstOrDefault(w => w.Id == id && w.Plan.UserId == user.Id);
            if (week == null) return NotFound("Week not found or access denied");

            _context.Weeks.Remove(week);
            _context.SaveChanges();
            return Ok(new { message = "Week deleted" });
        }
    }

    // DTOs
    public class CreateWeekDto
    {
        public int WeekNumber { get; set; } = 0; // 0 => use next automatic
        public string? Summary { get; set; }
        public double Progress { get; set; } = 0.0;
    }

    public class UpdateWeekDto
    {
        public int? WeekNumber { get; set; }
        public string? Summary { get; set; }
        public double? Progress { get; set; }
    }
}
