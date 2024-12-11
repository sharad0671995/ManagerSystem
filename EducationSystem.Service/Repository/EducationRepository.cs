using AutoMapper;
using EducationSystem.Entity.DTO.Education;
using EducationSystem.Entity.Models;
using EducationSystem.Service.Context;
using EducationSystem.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Service.Repository
{
    public class EducationRepository :IEducation
    {


        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public EducationRepository(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EducationReadDto>> GetAllEducationsAsync()
        {
            var educations = await _context.Educations
                .Include(e => e.Degree)
                .Include(e => e.Institution)
                .ToListAsync();

            return _mapper.Map<List<EducationReadDto>>(educations);
        }

        public async Task<EducationReadDto> GetEducationByIdAsync(Guid id)
        {
            var education = await _context.Educations
                .Include(e => e.Degree)
                .Include(e => e.Institution)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (education == null)
                throw new KeyNotFoundException("Education not found");

            return _mapper.Map<EducationReadDto>(education);
        }

        public async Task<Guid> CreateEducationAsync(EducationCreateDto educationCreateDto)
        {
            var education = _mapper.Map<Education>(educationCreateDto);
            education.Id = Guid.NewGuid(); // Generate a new ID for the entity

            _context.Educations.Add(education);
            await _context.SaveChangesAsync();

            return education.Id;
        }

        public async Task<bool> UpdateEducationAsync(EducationUpdateDto educationUpdateDto)
        {
            var education = await _context.Educations.FindAsync(educationUpdateDto.Id);

            if (education == null)
                throw new KeyNotFoundException("Education not found");

            _mapper.Map(educationUpdateDto, education);

            _context.Educations.Update(education);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteEducationAsync(Guid id)
        {
            var education = await _context.Educations.FindAsync(id);

            if (education == null)
                throw new KeyNotFoundException("Education not found");

            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
