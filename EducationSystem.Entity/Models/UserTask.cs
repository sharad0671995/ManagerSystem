using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EducationSystem.Entity.Models
{
    public class UserTask
    {
        /* [Key]
         public int Id { get; set; }  // Unique identifier for the task

         public string? Title { get; set; }  // Title of the task

         public string? Description { get; set; }  // Detailed description of the task

         public DateTime DueDate { get; set; }  // Due date of the task

         public bool IsCompleted { get; set; }  // Whether the task is completed or not

         // Enum for the task status (You can choose to uncomment it if necessary)
         public TaskStatus Status { get; set; } = TaskStatus.NotStarted;  // Default value is NotStarted

         // Foreign key to the user who created the task (using AppUser)
         public string CreatedById { get; set; }  // Foreign key for AppUser (IdentityUser has a string primary key by default)
         public AppUser? CreatedBy { get; set; }  // Navigation property to the creator

         // Foreign key to the user who the task is assigned to (using AppUser)
         public string AssignedToId { get; set; }  // Foreign key for AppUser (IdentityUser has a string primary key by default)
         public AppUser? AssignedTo { get; set; }  // Navigation property to the assignee
        */



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        // Task Status Enum
        public TaskStatus Status { get; set; } = TaskStatus.Submitted;

        // Foreign Keys for Users
        public string? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }

        public string? AssignedToId { get; set; }
        public AppUser? AssignedTo { get; set; }

        public string? Priority { get; set; } // High, Medium, Low

       
        }

    
    // Enum for task status
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskStatus
    {
        Submitted,   // Task is not started yet
        InProgress,   // Task is currently being worked on
        Completed,    // Task is completed
        OnHold,       // Task is paused or put on hold
        Cancelled,    // Task has been cancelled
        Expired       // Task has expired
    }
}


