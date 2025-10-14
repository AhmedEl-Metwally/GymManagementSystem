using GymManagementBLL.Services.Interface;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Implementation
{
    public class MemberService( 
                                IGenericRepository<Member> _memberRepository,
                                IGenericRepository<MemberPlan> _memberPlanRepository,
                                IGenericRepository<HealthRecord> _healthRecordRepository,
                                IPlanRepository _planRepository
                              ) : IMemberService
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
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone))
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

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var Member = _memberRepository.GetById(MemberId);
            if(Member is null)
                return null;

            var viweModel = new MemberViewModel()
            {
                Name = Member.Name,
                Phone = Member.Phone,
                Email = Member.Email,
                Photo = Member.Photo,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Address = $"{Member.Address.BuildingNumber}-{Member.Address.Street}-{Member.Address.City}" 
            };

            var ActiveMemberPlan = _memberPlanRepository.GetAll(M =>M.MemberId == MemberId && M.Status == "Active").FirstOrDefault();
            if (ActiveMemberPlan is not null)
            {
                viweModel.MembershipStartDate = ActiveMemberPlan.CreatedAt.ToShortDateString();
                viweModel.MembershipEndDate = ActiveMemberPlan.EndDate.ToShortDateString();
                var plan = _planRepository.GetPlanById(ActiveMemberPlan.PlanId);
                viweModel.PlanName = plan?.Name;
            }

            return viweModel;
        }

        public HealthRecordViewModel? GetHealthRecordDetails(int MemberId)
        {
            var HealthRecordRepository = _healthRecordRepository.GetById(MemberId);
            if (HealthRecordRepository is null)
                return null;

            return new HealthRecordViewModel()
            {
                Height = HealthRecordRepository.Height,
                Weight = HealthRecordRepository.Weight , 
                BloodType = HealthRecordRepository.BloodType,
                Note = HealthRecordRepository.Note
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _memberRepository.GetById(MemberId);   
            if (Member is null)
                return null;
            return new MemberToUpdateViewModel()
            {
                Email = Member.Email,
                Name = Member.Name,
                Phone = Member.Phone,
                Photo = Member.Photo,   
                City = Member.Address.City,
                Street = Member.Address.Street,
                BuildingNumber = Member.Address.BuildingNumber
            };
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel memberToUpdate)
        {
            try 
            {
                if (IsEmailExists(memberToUpdate.Email) || IsPhoneExists(memberToUpdate.Phone))
                    return false;

                var Member = _memberRepository.GetById(id);
                if (Member is null) 
                    return false;

                Member.Email = memberToUpdate.Email;
                Member.Phone = memberToUpdate.Phone;
                Member.Address.BuildingNumber = memberToUpdate.BuildingNumber; 
                Member.Address.City = memberToUpdate.City; 
                Member.Address.Street = memberToUpdate.Street;
                Member.UpdatedAt = DateTime.Now;

                return  _memberRepository.Update(Member) > 0;
            } 
            catch
            {
                return false;
            }
        }




        //Help Methods
        private bool IsEmailExists(string email) => _memberRepository.GetAll(E => E.Email == email).Any();
        private bool IsPhoneExists(string phone) => _memberRepository.GetAll(E => E.Phone == phone).Any();
    }
}
