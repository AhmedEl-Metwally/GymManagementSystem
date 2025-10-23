﻿using AutoMapper;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;

namespace GymManagementBLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, option => option.MapFrom(src => src.Trainer.Name))
                .ForMember(dest =>dest.AvailableSlots, option => option.Ignore());
        }
    }

}
