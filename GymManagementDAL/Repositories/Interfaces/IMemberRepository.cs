using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAllMembers();
        Member? GetMemberById(int id);
        int AddMember(Member member);
        int UpdateMember(Member member);
        int DeleteMember(int id);
    }
}
