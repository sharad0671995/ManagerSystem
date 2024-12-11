using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Entity.DTO.Education
{
    public class EducationReadDto
    {
        public Guid Id { get; set; }                  // Primary key of the education record
        public string DegreeName { get; set; }        // Degree name
        public string InstitutionName { get; set; }   // Institution name
        public DateTime StartDate { get; set; }       // Start date of education
        public DateTime? EndDate { get; set; }        // End date (optional)
        public string FieldOfStudy { get; set; }      // Field of study
        public double? GPA { get; set; }              // GPA (optional)
        public string UserId { get; set; }            // User ID
    }

}
