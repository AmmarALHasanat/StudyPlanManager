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
                //.Include(p=>p.User)
                //.AsNoTracking()
                .Where(p => p.UserId == user.Id)
                .Select(p => new Plan
                {
                    Id = p.Id, 
                    Name = p.Name,
                    DurationWeeks = p.DurationWeeks,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Weeks = p.Weeks,
                    UserId = p.UserId,
                    //User = new User {Id=p.UserId, Username= p.User!.Username,Email=p.User!.Email} 
                })
                .ToList();
            
            return Ok(plans);
        }

        [HttpPost]
        public IActionResult AddPlan([FromBody] PlanDto plan)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return Unauthorized("User not found");

            Plan newPlan = new Plan
            {
                Name = plan.Name,
                Description = plan.Description,
                StartDate = plan.StartDate,
                EndDate = plan.EndDate,
                DurationWeeks = plan.DurationWeeks
            };
            newPlan.UserId = user.Id;
            _context.Plans.Add(newPlan);
            _context.SaveChanges();

            return Ok(plan);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePlan(int id, [FromBody] PlanDto updated)
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
    // or Use this Object to return only necessary fields 
    public class PlanDto
    {
        public string Name { get; set; } = string.Empty;
        public int DurationWeeks { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
