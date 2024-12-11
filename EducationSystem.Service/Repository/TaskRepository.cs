using AutoMapper;
using EducationSystem.Entity.DTO.Task;
using EducationSystem.Entity.Models;
using EducationSystem.Service.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskStatus = EducationSystem.Entity.DTO.Task.TaskStatus;

namespace EducationSystem.Service.Repository
{
    public class TaskRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public TaskRepository(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserTask> AddTaskAsync(TaskCreateDto taskCreateDto)
        {
            if (taskCreateDto == null)
                throw new ArgumentNullException(nameof(taskCreateDto));

            // Validate: Check if the CreatedByUser and AssignedToUser exist
            var createdByUser = await _context.Users.FindAsync(taskCreateDto.CreatedById);
            var assignedToUser = await _context.Users.FindAsync(taskCreateDto.AssignedToId);

            if (createdByUser == null || assignedToUser == null)
                throw new ArgumentException("Invalid user(s).");

            // Determine the task status based on IsCompleted and DueDate
           // taskCreateDto.Status = GetTaskStatus(taskCreateDto);

            // Map TaskCreateDto to UserTask
            var userTask = _mapper.Map<UserTask>(taskCreateDto);

            try
            {
                // Add the new task to the database
                await _context.Tasks.AddAsync(userTask);
                await _context.SaveChangesAsync();

                return userTask; // Return the created task
            }
            catch (Exception ex)
            {
                // Handle error (log it, rethrow it, or return a specific message)
                throw new InvalidOperationException("Error adding the task to the database.", ex);
            }
        }





        public async Task<UserTask> UpdateTaskAsync(TaskUpdatedDto taskUpdateDto)
        {
            if (taskUpdateDto == null)
                throw new ArgumentNullException(nameof(taskUpdateDto));

            // Find the task to be updated
            var existingTask = await _context.Tasks.FindAsync(taskUpdateDto.TaskId);

            if (existingTask == null)
                throw new KeyNotFoundException("Task not found.");

            // Validate the AssignedToUser
            var assignedToUser = await _context.Users.FindAsync(taskUpdateDto.AssignedToId);
            if (assignedToUser == null)
                throw new ArgumentException("Assigned user not found.");

            // Update status based on new details
            var updatedStatus = GetTaskStatus(new TaskCreateDto
            {
                DueDate = taskUpdateDto.DueDate,
                IsCompleted = taskUpdateDto.IsCompleted
            });

            // Map changes from TaskUpdateDto to existing Task
            _mapper.Map(taskUpdateDto, existingTask);
            existingTask.Status = (Entity.Models.TaskStatus)updatedStatus;

            try
            {
                _context.Tasks.Update(existingTask);
                await _context.SaveChangesAsync();

                return existingTask;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating the task.", ex);
            }
        }


        public async Task<bool> DeleteTaskAsync(long taskId)
        {
            try
            {
                // Fetch the task explicitly using FirstOrDefaultAsync for flexibility
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

                if (task == null)
                    throw new KeyNotFoundException("Task not found.");

                _context.Tasks.Remove(task); // Mark the task for deletion
                await _context.SaveChangesAsync(); // Commit the changes

                return true; // Successfully deleted
            }
            catch (KeyNotFoundException)
            {
                throw; // Re-throw the "not found" exception to handle it at the controller level
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error deleting the task.", ex); // Handle unexpected errors
            }
        }


        // Helper method to determine TaskStatus based on IsCompleted and DueDate
        private TaskStatus GetTaskStatus(TaskCreateDto taskCreateDto)
        {
            if (taskCreateDto.IsCompleted)
                return TaskStatus.Completed;

            if (taskCreateDto.DueDate < DateTime.Now)
                return TaskStatus.Expired;

            if (taskCreateDto.DueDate >= DateTime.Now && taskCreateDto.DueDate <= DateTime.Now.AddDays(7))
                return TaskStatus.InProgress;

            return TaskStatus.NotStarted;
        }
    }
}
