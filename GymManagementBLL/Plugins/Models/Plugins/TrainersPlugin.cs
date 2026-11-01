namespace GymManagementBLL.Plugins.Models.Plugins
{
    public class TrainersPlugin(ITrainerService _trainerService)
    {
        [KernelFunction("get_trainers")]
        [Description("Returns all of the gym's trainers: the total count, and for each trainer the name and specialty. Use it for questions such as who the trainers are or how many trainers the gym has.")]
        public TrainersResult GetTrainers() => ToResult(_trainerService.GetAllTrainers());

        [KernelFunction("find_trainers_by_specialty")]
        [Description("Finds trainers by specialty. Use it for questions such as who teaches yoga or which trainers do boxing.")]
        public TrainersResult FindTrainersBySpecialty([Description("The specialty. One of: GeneralFitness, Yoga, Boxing, CrossFit")] string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                return new TrainersResult(0, []);
            return ToResult(_trainerService.GetAllTrainers().Where(T => T.Specialties.Contains(specialty, StringComparison.OrdinalIgnoreCase)));
        }

        // Helper method
        private static TrainersResult ToResult(IEnumerable<TrainerViewModel> trainers)
        {
            var trainerInfos = trainers.Select(T => new TrainerInfo(T.Name, T.Specialties)).ToList();
            return new TrainersResult(trainerInfos.Count, trainerInfos);
        }
    }
}
