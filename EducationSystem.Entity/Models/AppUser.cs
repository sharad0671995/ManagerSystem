using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Entity.Models
{
    public class AppUser : IdentityUser
    {




        // public string? FirstName { get; set; }
        // public string? LastName { get; set; }
        // public string FullName => $"{FirstName} {LastName}";
        public string? FullName { get; set; }

       // public int Age { get; set; }

      public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();  // Tasks created by the user
      public ICollection<UserTask> AssignedTasks { get; set; } = new List<UserTask>();  // Tasks assigned to the user
                                                                                        // Add Notifications Navigation Property
                                                                                        // Navigation property for related education records
        public ICollection<Education> Educations { get; set; }                                                                                 // public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
