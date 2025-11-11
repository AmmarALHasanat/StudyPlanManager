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
    public class PlanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlanController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMyPlans()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized("User not found");

            var plans = _context.Plans
                .Include(p => p.Weeks)
                .Where(p => p.UserId == user.Id)
                .ToList();

            return Ok(plans);
        }

        [HttpPost]
        public IActionResult AddPlan([FromBody] Plan plan)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized("User not found");

            plan.UserId = user.Id;
            _context.Plans.Add(plan);
            _context.SaveChanges();

            return Ok(plan);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePlan(int id, [FromBody] Plan updated)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized("User not found");

            var plan = _context.Plans.FirstOrDefault(p => p.Id == id && p.UserId == user.Id);
            if (plan == null) return NotFound("Plan not found");

            plan.Name = updated.Name;
            plan.Description = updated.Description;
            plan.StartDate = updated.StartDate;
            plan.EndDate = updated.EndDate;
            plan.DurationWeeks = updated.DurationWeeks;

            _context.SaveChanges();
            return Ok(plan);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlan(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized("User not found");

            var plan = _context.Plans.FirstOrDefault(p => p.Id == id && p.UserId == user.Id);
            if (plan == null) return NotFound("Plan not found");

            _context.Plans.Remove(plan);
            _context.SaveChanges();

            return Ok(new { message = "Plan deleted" });
        }
    }
}
