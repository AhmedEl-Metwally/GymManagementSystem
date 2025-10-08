using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ITrainerRepository
    {
        IEnumerable<Trainer> GetAllTrainers();
        Trainer? GetTrainerById(int id);
        int AddTrainer(Trainer trainer);
        int UpdateTrainer(Trainer trainer);
        int DeleteTrainer(Trainer trainer);
    }
}
