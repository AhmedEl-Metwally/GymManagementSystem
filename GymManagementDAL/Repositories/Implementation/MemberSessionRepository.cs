using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Implementation
{
    public class MemberSessionRepository : GenericRepository<MemberSession>,IMemberSessionRepository
    {
        private readonly GymDbContext _context;
        public MemberSessionRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<MemberSession> GetMemberSessionById(int sessionId)
            => _context.MemberSessions.Where(MS => MS.SessionId == sessionId).Include(MS =>MS.Member).ToList();

    }
}
