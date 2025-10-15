using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.UnitOfWorks;

namespace GymManagementBLL.Services.Implementation
{
    public class PlanService(IUnitOfWork _unitOfWork) : IPlanService
    {
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any())
                return [];

            return plans.Select(P => new PlanViewModel()
            {
                Id = P.Id,  
                Name = P.Name,  
                Description = P.Description,    
                DurationDays = P.DurationDays,  
                Price = P.Price,    
                IsActive = P.IsActive,
            });
        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if(Plan is null )
                return null;

            return new PlanViewModel()
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price,
                IsActive = Plan.IsActive,
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if(Plan is null || Plan.IsActive || HasActiveMemberPlan(planId))
                return null;
            return new UpdatePlanViewModel()
            {
                Description = Plan.Description, 
                DurationDays= Plan.DurationDays,    
                Price = Plan.Price, 
                PlanName = Plan.Name,
            };
        }

        public bool UpdatePlan(int planId, UpdatePlanViewModel updatePlan)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var Plan = PlanRepo.GetById(planId);
            if (Plan is null || HasActiveMemberPlan(planId))
                return false;

            try
            {
                (Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt) = (updatePlan.Description, updatePlan.Price, updatePlan.DurationDays, DateTime.Now);
                PlanRepo.Update(Plan);
                return _unitOfWork.SaveChange() > 0;
            }
            catch 
            {
                return false;
            }
         
        }

        //SoftDelete
        public bool ToggleStatus(int planId)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var Plan = PlanRepo.GetById(planId);
            if (Plan is null || HasActiveMemberPlan(planId))
                return false;

            Plan.IsActive = Plan.IsActive == true? false: true;
            Plan.UpdatedAt = DateTime.Now;

            try
            {
                PlanRepo.Update(Plan);
                return _unitOfWork.SaveChange() > 0;
            }
            catch
            {
                return false;
            }
        }

      

        //Helper
        private bool HasActiveMemberPlan(int planId)
        {
            var ActiveMemberPlan = _unitOfWork.GetRepository<MemberPlan>()
                .GetAll( M => M.PlanId == planId && M.Status == "Active");
            return ActiveMemberPlan.Any();
        }


    }
}
