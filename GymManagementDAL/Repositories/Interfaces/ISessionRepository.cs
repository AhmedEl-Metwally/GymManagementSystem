using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session> 
    {
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategory();
        int GetCountOfMemberSessionSlots(int sessionId);
        Session? GetSessionWithTrainerAndCategory(int SessionId);
    }
}
