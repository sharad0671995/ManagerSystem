using System;
using System.ComponentModel.DataAnnotations;
namespace EducationSystem.Entity.DTO.Task
{
    // Enum to define the possible task statuses
    // Enum to define the possible task statuses
    public enum TaskStatus : int
    {
        NotStarted,   // Task is not started yet
        InProgress,   // Task is currently being worked on
        Completed,    // Task is completed
        OnHold,       // Task is paused or put on hold
        Cancelled,    // Task has been cancelled
        Overdue,      // Task is overdue
        Expired       // Task is expired after a specific duration
    }



    public class TaskCreateDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "CreatedById is required.")]
        public string? CreatedById { get; set; }

        [Required(ErrorMessage = "AssignedToId is required.")]
        public string? AssignedToId { get; set; }

        [Required(ErrorMessage = "IsCompleted status is required.")]
        public bool IsCompleted { get; set; }

        // Duration in days after the due date to mark the task as expired
        public int ExpirationDurationInDays { get; set; } = 7;
        public string? Priority { get; set; }
        public TaskStatus Status => GetTaskStatus();
        // Private method to calculate the task status based on the logic
        private TaskStatus GetTaskStatus()
        {
            if (IsCompleted)
                return TaskStatus.Completed;

            // Check if the current date exceeds the expiration duration
            if (DateTime.UtcNow > DueDate.AddDays(ExpirationDurationInDays))
                return TaskStatus.Expired;

            if (DueDate < DateTime.UtcNow)
                return TaskStatus.Overdue; // Task is overdue but within the expiration period

            return TaskStatus.InProgress;
        }
    }
}
