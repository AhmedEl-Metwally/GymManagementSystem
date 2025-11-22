using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberPlanRepository : IGenericRepository<MemberPlan>
    {
        IEnumerable<MemberPlan> GetAllMemberPlanWithMembersAndPlans(Func<MemberPlan,bool>? filter = null);
        MemberPlan? GetFirstOrDefault(Func<MemberPlan, bool>? filter = null);
    }
}
