namespace GymManagementBLL.Services.Interface
{
    public interface IServiceManager
    {
        IAccountService AccountService { get; }
        IAnalyticsService AnalyticsService { get; }
        IMemberPlanService MemberPlanService { get; }
        IMemberService MemberService { get; }
        IMemberSessionService MemberSessionService { get; }
        IPlanService PlanService { get; }
        ISessionService SessionService { get; }
        ITrainerService TrainerService { get; }
        IAttachmentService AttachmentService { get; }
        IGymAssistantService GymAssistantService { get; }
    }
}
