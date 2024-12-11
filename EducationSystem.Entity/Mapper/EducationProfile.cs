using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EducationSystem.Entity.DTO.Education;
using EducationSystem.Entity.Models;


namespace EducationSystem.Entity.Mapper
{
    
    public class EducationProfile : Profile
    {
        public EducationProfile()
        {
            CreateMap<Education, EducationReadDto>()
                .ForMember(dest => dest.DegreeName, opt => opt.MapFrom(src => src.Degree.Name))
                .ForMember(dest => dest.InstitutionName, opt => opt.MapFrom(src => src.Institution.Name));

            CreateMap<EducationCreateDto, Education>();
            CreateMap<EducationUpdateDto, Education>();
        }
    }

}
