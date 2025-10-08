using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementDAL.Repositories.Implementation
{
    public class TrainerRepository(GymDbContext _context) : ITrainerRepository
    {
        public int AddTrainer(Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            return _context.SaveChanges();
        }

        public int DeleteTrainer(Trainer trainer)
        {
            _context.Trainers.Remove(trainer);
            return _context.SaveChanges();
        }

        public IEnumerable<Trainer> GetAllTrainers() => _context.Trainers.ToList();
 
        public Trainer? GetTrainerById(int id) => _context.Trainers.Find(id);

        public int UpdateTrainer(Trainer trainer)
        {
            _context.Trainers.Update(trainer);
            return _context.SaveChanges();
        }
    }
}
