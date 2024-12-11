using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Entity.DTO.Education
{
    public class EducationCreateDto
    {
        public Guid DegreeId { get; set; }            // Reference to the degree
        public Guid InstitutionId { get; set; }       // Reference to the institution
        public DateTime StartDate { get; set; }       // Start date of education
        public DateTime? EndDate { get; set; }        // End date (optional)
        public string FieldOfStudy { get; set; }      // Field of study
        public double? GPA { get; set; }              // GPA (optional)
        public string UserId { get; set; }            // User ID
    }

}
