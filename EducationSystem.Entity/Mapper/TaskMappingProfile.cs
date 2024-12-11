using AutoMapper;
using EducationSystem.Entity.DTO.Task;
using EducationSystem.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Entity.Mapper
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskCreateDto, UserTask>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Entity.Models.TaskStatus)src.Status));
        }
    }
}
