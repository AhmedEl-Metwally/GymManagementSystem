using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Implementation;
using GymManagementDAL.Repositories.Interfaces;


namespace GymManagementDAL.Repositories.UnitOfWorks
{
    public class UnitOfWork(GymDbContext _context, ISessionRepository sessionRepository) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];

        public ISessionRepository SessionRepository { get; } = sessionRepository;

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType, out var Repo))
                return (IGenericRepository<TEntity>)Repo;

            var NewRepo = new GenericRepository<TEntity>(_context);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChange() => _context.SaveChanges();
      
    }
}
