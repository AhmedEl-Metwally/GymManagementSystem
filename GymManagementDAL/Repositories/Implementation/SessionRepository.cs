using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Implementation
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory() =>
            _context.Sessions.Include(S => S.Trainer).Include(S => S.Category).ToList();

        public int GetCountOfBookedSlots(int sessionId) =>
            _context.MemberSessions.Count(MS => MS.SessionId == sessionId);

        public Session? GetSessionWithTrainerAndCategory(int SessionId) =>
            _context.Sessions.Include(S => S.Trainer).Include(S => S.Category).FirstOrDefault(S =>S.Id == SessionId);
      
    }
}
