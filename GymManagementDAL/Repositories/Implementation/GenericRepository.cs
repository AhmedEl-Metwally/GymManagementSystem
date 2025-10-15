using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Implementation
{
    public class GenericRepository<TEntity>(GymDbContext _context) : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if(condition is null)
                return _context.Set<TEntity>().AsNoTracking().ToList();
            else
                return _context.Set<TEntity>().AsNoTracking().Where(condition).ToList();
        }

        public TEntity? GetById(int id) => _context.Set<TEntity>().Find( id);

        public void Add(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

    }
}
