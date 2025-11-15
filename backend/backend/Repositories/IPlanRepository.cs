using backend.DTO;
using backend.Models;

namespace backend.Repositories
{
    public interface IPlanRepository
    {
        IEnumerable<Plan> GetPlansByUserId(int userId);
        Plan AddPlan(PlanDto plan, int userId);
        Plan UpdatePlan(Plan plan,PlanDto planDto);
        void RemovePlan(Plan plan);
        Plan? GetPlanById(int id,int userId);

    }
}
