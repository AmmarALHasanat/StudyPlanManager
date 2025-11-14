using backend.DTO;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlanController : ControllerBase
    {
        private readonly IPlanRepository _planRepository;
        private readonly IUserRepository _userRepository;

        public PlanController( IPlanRepository planRepository,IUserRepository userRepository)
        {
            _planRepository = planRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetMyPlans()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _userRepository.GetByUsername(username??"");
            if (user == null) return Unauthorized("User not found");

            var plans = _planRepository.GetPlansByUserId(user.Id);

            return Ok(plans);
        }

        [HttpPost]
        public IActionResult AddPlan([FromBody] PlanDto plan)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _userRepository.GetByUsername(username ?? "");
            if (user == null) return Unauthorized("User not found");


           var newPlan = _planRepository.AddPlan(plan, user.Id);
            return Ok(plan);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePlan(int id, [FromBody] PlanDto updated)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _userRepository.GetByUsername(username ?? "");
            if (user == null) return Unauthorized("User not found");

            var plan = _planRepository.GetPlanById(id, user.Id);
            if (plan == null) return NotFound("Plan not found");
            _planRepository.UpdatePlan(plan, updated);

            return Ok(plan);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePlan(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = _userRepository.GetByUsername(username ?? "");
            if (user == null) return Unauthorized("User not found");

            var plan = _planRepository.GetPlanById(id, user.Id);
            if (plan == null) return NotFound("Plan not found");
            _planRepository.RemovePlan(plan);

            return Ok(new { message = "Plan deleted" });
        }
    }
}
