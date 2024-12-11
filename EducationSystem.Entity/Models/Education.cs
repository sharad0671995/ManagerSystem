using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Entity.Models
{
    public class Education
    {
        public Guid Id { get; set; }                // Primary key

        public Guid DegreeId { get; set; }           // Foreign key for Degree
        public Degree Degree { get; set; }          // Navigation property

        public Guid InstitutionId { get; set; }      // Foreign key for Institution
        public Institution Institution { get; set; } // Navigation property

        public DateTime StartDate { get; set; }     // Start date of the education
        public DateTime? EndDate { get; set; }      // End date (nullable)
        public string FieldOfStudy { get; set; }    // Field of study (e.g., IT)
        public double? GPA { get; set; }            // Grade Point Average

        // Foreign key to AspNetUsers
        public string UserId { get; set; }
        public IdentityUser User { get; set; }      // Navigation property
    }

    public class Degree
    {
        public Guid Id { get; set; }            // Primary key
        public string Name { get; set; }       // Degree name (e.g., "B.Sc.", "M.Sc.")
    }

    public class Institution
    {
        public Guid Id { get; set; }            // Primary key
        public string Name { get; set; }       // Institution name (e.g., "XYZ University")
    }

}
