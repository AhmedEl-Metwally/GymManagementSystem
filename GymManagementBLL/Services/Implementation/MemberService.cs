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


        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                var emailExists = _memberRepository.GetAll(E => E.Email == createMember.Email).Any();
                var phoneExists = _memberRepository.GetAll(P => P.Phone == createMember.Phone).Any();
                if (emailExists || phoneExists)
                    return false;

                var member = new Member()
                {
                    Phone = createMember.Phone,
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        Street = createMember.Street,
                        City = createMember.City,
                        BuildingNumber = createMember.BuildingNumber,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note,
                    },
                };
                return _memberRepository.Add(member) > 0;
            }
            catch (Exception) 
            {
                return false;
            }
        }






    }
}
