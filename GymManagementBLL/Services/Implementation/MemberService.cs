using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Implementation
{
    public class MemberService(IGenericRepository<Member> _memberRepository) : IMemberService
    {
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Menbers = _memberRepository.GetAll();
            if (Menbers is null || !Menbers.Any())
                return Enumerable.Empty<MemberViewModel>();

            var MemberViewModels = Menbers.Select(M => new MemberViewModel 
            {
                Id = M.Id,
                Name = M.Name,
                Email = M.Email,
                Phone = M.Phone,
                Photo = M.Photo,
                Gender = M.Gender.ToString(),

            });

            return MemberViewModels;
        }








    }
}
