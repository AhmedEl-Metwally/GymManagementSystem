namespace GymManagementBLL.Services.Implementation
{
    public class ServiceManager(IAccountService _accountService, IAnalyticsService _analyticsService,
        IMemberPlanService _memberPlanService, IMemberService _memberService, IMemberSessionService _memberSessionService,
        IPlanService _planService, ISessionService _sessionService, ITrainerService _trainerService,
        IAttachmentService _attachmentService, IGymAssistantService _gymAssistantService
        ) : IServiceManager
    {
        public IAccountService AccountService => _accountService;

        public IAnalyticsService AnalyticsService => _analyticsService;

        public IMemberPlanService MemberPlanService => _memberPlanService;

        public IMemberService MemberService => _memberService;

        public IMemberSessionService MemberSessionService => _memberSessionService;

        public IPlanService PlanService => _planService;

        public ISessionService SessionService => _sessionService;

        public ITrainerService TrainerService => _trainerService;

        public IAttachmentService AttachmentService => _attachmentService;

        public IGymAssistantService GymAssistantService => _gymAssistantService;
    }
}
