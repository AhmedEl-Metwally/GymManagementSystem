using GymManagementBLL.ViewModels.MemberViewModels;

namespace GymManagementBLL.Services.Interface
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel createMember);
    }
}
