using GymManagementBLL.ViewModels.MemberViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel createMember);
        MemberViewModel? GetMemberDetails(int MemberId);
        HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId);
        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
        bool UpdateMemberDetails(int id,MemberToUpdateViewModel memberToUpdate);
        bool RenewMember(int MemberId);
    }
}
