using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Implementation
{
    public class MemberPlanRepository : GenericRepository<MemberPlan>,IMemberPlanRepository
    {
        private readonly GymDbContext _context;

        public MemberPlanRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<MemberPlan> GetAllMemberPlanWithMembersAndPlans(Func<MemberPlan, bool>? filter = null)
        {
            var memberPlan = _context.MemberPlans.Include(M => M.Member)
                                                               .Include(M => M.Plan)
                                                               .Where(filter ?? (_ => true)).ToList();
            return memberPlan;
        }

        public MemberPlan? GetFirstOrDefault(Func<MemberPlan, bool>? filter = null)
        {
            var memberPlan = _context.MemberPlans.Include(M => M.Member)
                                                          .Include(M => M.Plan)
                                                          .FirstOrDefault(filter ?? (_ => true));
            return memberPlan;
        }
    }
}
