using EducationSystem.Entity.DTO.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Service.Interface
{
    public interface IEducation
    {
        Task<List<EducationReadDto>> GetAllEducationsAsync();
        Task<EducationReadDto> GetEducationByIdAsync(Guid id);
        Task<Guid> CreateEducationAsync(EducationCreateDto educationCreateDto);
        Task<bool> UpdateEducationAsync(EducationUpdateDto educationUpdateDto);
        Task<bool> DeleteEducationAsync(Guid id);
    }
}
