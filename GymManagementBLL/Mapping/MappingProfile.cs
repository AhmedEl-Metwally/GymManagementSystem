using AutoMapper;
using GymManagementBLL.ViewModels.MemberPlanViewModels;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;

namespace GymManagementBLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
            MapMember();
            MapTrainer();
            MapPlan();
            MemberPlan();
        }


        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, option => option.MapFrom(src => src.Trainer.Name))
                .ForMember(dest =>dest.AvailableSlots, option => option.Ignore());

            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest =>dest.Name,option =>option.MapFrom(src =>src.CategoryName));

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session,UpdateSessionViewModel>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
        }

        private void MapMember()
        {
            CreateMap<CreateMemberViewModel,Member>()
                .ForMember(dest => dest.Address, option => option.MapFrom(src => src))
                .ForMember(dest => dest.HealthRecord,option => option.MapFrom(src => src.HealthRecordViewModel));

            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(dest => dest.BuildingNumber,option => option.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.Street,option =>option.MapFrom(src =>src.Street))
                .ForMember(dest => dest.City,option =>option.MapFrom(src =>src.City));

            CreateMap<Member,MemberViewModel>()
                .ForMember(dest => dest.Gender,option =>option.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest =>dest.DateOfBirth,option => option.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address,option => option.MapFrom(src => $"{src.Address.BuildingNumber}-{src.Address.Street}-{src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, option => option.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, option => option.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, option => option.MapFrom(src => src.Address.City));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, option => option.Ignore())
                .ForMember(dest => dest.Phone, option => option.Ignore())
                .AfterMap((src, dest) => 
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;
                });

            CreateMap<HealthRecordViewModel,HealthRecord>().ReverseMap();
        }

        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, option => option.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));

            CreateMap<Trainer, TrainerViewModel>();

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dist => dist.Street, option => option.MapFrom(src => src.Address.Street))
                .ForMember(dist => dist.City, option => option.MapFrom(src => src.Address.City))
                .ForMember(dist => dist.BuildingNumber, option => option.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.UpdatedAt = DateTime.Now;
            });
        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, option => option.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
           .ForMember(dest => dest.Name, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedAt, option => option.MapFrom(src => DateTime.Now));
                
        }

        private void MemberPlan()
        {
            CreateMap<MemberPlan, MemberPlanViewModel>()
                .ForMember(dest => dest.MemberName,option => option.MapFrom(src =>src.Member.Name))
                .ForMember(dest => dest.PlanName,option => option.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.StartDate,option =>option.MapFrom(src =>src.CreatedAt));
        }

    }

}
