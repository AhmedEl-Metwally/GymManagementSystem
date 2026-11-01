namespace GymManagementBLL.Services.Interface
{
    public interface IMemberSessionService
    {
        IEnumerable<SessionViewModel> GetAllMemberSessionWithTrainerAndCategory();
        IEnumerable<MemberForSessionViewModel> GetAllMemberSession(int sessionId);
        bool CreateMemberSession(CreateMemberSessionViewModel createMemberSessionViewModel);
        IEnumerable<MemberForSelectListViewModel> GetMembersForDropdown(int sessionId);
        bool MemberAttended(MemberAttendOrCancelViewModel memberAttendOrCancel);
        bool CancelMemberSession(MemberAttendOrCancelViewModel memberAttendOrCancel);
    }
}
