using backend.Controllers;
using backend.Data;
using backend.DTO;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace backend.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly ApplicationDbContext _context;
                public PlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Plan> GetPlansByUserId(int userId)
        {
            return _context.Plans
                .Include(p => p.Weeks)
                //.Include(p=>p.User)
                //.AsNoTracking()
                .Where(p => p.UserId == userId)
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
        }
        public Plan AddPlan(PlanDto plan, int userId)
        {
            Plan newPlan = new Plan
            {
                Name = plan.Name,
                Description = plan.Description,
                StartDate = plan.StartDate,
                EndDate = plan.EndDate,
                DurationWeeks = plan.DurationWeeks
            };
            newPlan.UserId = userId;
            _context.Plans.Add(newPlan);
            _context.SaveChanges();
            return newPlan;
        }
        public void UpdatePlan(Plan plan)
        {
            _context.Plans.Update(plan);
            _context.SaveChanges();
        }
        public Plan? GetPlanById(int id,int userId)
        {

            return _context.Plans
                .Include(p => p.Weeks)
                .FirstOrDefault(p => p.Id == id && p.UserId == userId); ;
        }

        public Plan UpdatePlan(Plan plan, PlanDto updated)
        {
            plan.Name = updated.Name;
            plan.Description = updated.Description;
            plan.StartDate = updated.StartDate;
            plan.EndDate = updated.EndDate;
            plan.DurationWeeks = updated.DurationWeeks;

            _context.SaveChanges();
            return plan;
        }

        public void RemovePlan(Plan plan)
        {
            _context.Plans.Remove(plan);
            _context.SaveChanges();
        }
    }
}
