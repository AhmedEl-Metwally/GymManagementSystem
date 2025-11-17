using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.UnitOfWorks
{
    public interface IUnitOfWork
    {
        public ISessionRepository SessionRepository  { get; }
        public IMemberPlanRepository MemberPlanRepository { get; }
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity :BaseEntity, new();
        int SaveChange();
    }
}
