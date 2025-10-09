using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Implementation
{
    public class SessionRepository(GymDbContext _context) : ISessionRepository
    {
        public int AddSession(Session session)
        {
            _context.Sessions.Add(session);
            return _context.SaveChanges();
        }

        public int DeleteSession(Session session)
        {
            _context.Sessions.Remove(session);
            return _context.SaveChanges();
        }

        public IEnumerable<Session> GetAllSessions() => _context.Sessions.ToList();

        public Session? GetSessionById(int id) => _context.Sessions.Find(id);

        public int UpdateSession(Session session)
        {
            _context.Sessions.Update(session);
            return _context.SaveChanges();  
        }
    }
}
