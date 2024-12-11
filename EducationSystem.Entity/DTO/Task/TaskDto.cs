using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationSystem.Entity.DTO.Task
{
    public class TaskDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string? CreatedByName { get; set; }
        public string? AssignedToName { get; set; }
        public TaskStatus Status { get; set; }  // Add Status property

        public string? Priority { get; set; } // High, Medium, Low

    }
}
