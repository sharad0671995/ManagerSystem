using AutoMapper;
using EducationSystem.Service.Context;
using EducationSystem.Service.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationSystem.Controllers
{

    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class EducationController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        
        private readonly IMapper _mapper;
        public EducationController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
           
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            EducationRepository educationRepository = new EducationRepository(_context, _mapper);
            var educations = await educationRepository.GetAllEducationsAsync();
            return Ok(educations);
        }


    }
}
