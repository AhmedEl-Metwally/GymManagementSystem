using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        Session? GetSessionById(int id);
        IEnumerable<Session> GetAllSessions();
        int AddSession(Session session);
        int UpdateSession(Session session);
        int DeleteSession(Session session);
    }
}
