using AutoMapper;
using EducationSystem.Entity.DTO.Task;
using EducationSystem.Service.Context;
using EducationSystem.Service.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskManagerController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        // private readonly TaskRepository _taskRepository;
        private readonly IMapper _mapper;
        public TaskManagerController(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            //_taskRepository = taskRepository;
             _mapper = mapper;
        }




        // Example method to retrieve a task by ID (GET method)
        
        [HttpPost]
        [Route("TaskCreate")]
        public async Task<IActionResult> CreateTask(TaskCreateDto taskCreateDto)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TaskRepository taskRepository = new TaskRepository(_context,_mapper);
            await taskRepository.AddTaskAsync(taskCreateDto);
            return Ok(new { message = $"Task Create successfully." });

        }

        /*  [HttpGet]
          [Route("ViewTask")]
          public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
          {
              var query = _context.Tasks
                  .Include(t => t.CreatedBy)
                  .Include(t => t.AssignedTo)
                  .AsNoTracking()
                  .OrderBy(t => t.DueDate); // Sorting by DueDate or another field

              // Implement pagination
              var tasks = await query
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .Select(t => new TaskDto
                  {
                      Id = t.Id,
                      Title = t.Title,
                      Description = t.Description,
                      DueDate = t.DueDate,
                      IsCompleted = t.IsCompleted,
                      CreatedByName = t.CreatedBy.FullName,
                      AssignedToName = t.AssignedTo.FullName,
                      Status = GetTaskStatus(t.IsCompleted, t.DueDate)  // Use static method
                  })
                  .ToListAsync();

              return Ok(tasks);
          }


          // Helper method to determine TaskStatus
          // Make the method static so it can be used in LINQ queries
           private static TaskStatus GetTaskStatus(bool isCompleted, DateTime dueDate)
            {
                if (isCompleted)
                    return TaskStatus.Completed;

                if (dueDate < DateTime.Now)
                    return TaskStatus.Overdue;

                return TaskStatus.InProgress;
            }

            */

        // Helper method to determine TaskStatus
        // Make the method static so it can be used in LINQ queries

       /* [HttpGet]
        [Route("ViewTask")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int expirationDurationInDays = 7 // Default expiration duration
        )
        {
            // Query tasks with necessary includes and ordering
            var query = _context.Tasks
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .AsNoTracking()
                .OrderBy(t => t.DueDate); // Sorting by DueDate

            // Implement pagination
            var tasks = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    CreatedByName = t.CreatedBy.FullName,
                    AssignedToName = t.AssignedTo.FullName,
                    Status = GetTaskStatus(t.IsCompleted, t.DueDate, expirationDurationInDays) // Use static method
                })
                .ToListAsync();

            return Ok(tasks);
        }
       */
        // Helper method to determine TaskStatus
        private static Entity.DTO.Task.TaskStatus GetTaskStatus(bool isCompleted, DateTime dueDate, int expirationDurationInDays)
        {
            if (isCompleted)
                return Entity.DTO.Task.TaskStatus.Completed;

            // Check if the task has expired based on the expiration duration
            if (DateTime.UtcNow > dueDate.AddDays(expirationDurationInDays))
                return Entity.DTO.Task.TaskStatus.Expired;

            // Check if the task is overdue but not expired
            if (dueDate < DateTime.UtcNow)
                return Entity.DTO.Task.TaskStatus.Overdue;

            return Entity.DTO.Task.TaskStatus.InProgress; // Default status if not completed, overdue, or expired
        }


        // PUT: api/Tasks/{taskId}
        [HttpPut("{taskId:long}")]
        public async Task<IActionResult> UpdateTask(long taskId, [FromBody] TaskUpdatedDto updatedTaskDto)
        {
            if (updatedTaskDto == null)
                return BadRequest("Task data cannot be null.");

            if (taskId != updatedTaskDto.TaskId)
                return BadRequest("The Task ID in the URL does not match the Task ID in the body.");

            try
            {
                TaskRepository taskRepository = new TaskRepository(_context, _mapper);
                // Use the injected repository (assumes dependency injection is properly configured)
                var updatedTask = await taskRepository.UpdateTaskAsync(updatedTaskDto);

                return Ok(updatedTask); // Return the updated task
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Task not found
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // DELETE: api/Tasks/{taskId}
       
        // DELETE: api/Tasks/{taskId}
        [HttpDelete("{taskId:long}")]
        public async Task<IActionResult> DeleteTask(long taskId)
        {
            try
            {
                // Use the task repository
                TaskRepository taskRepository = new TaskRepository(_context, _mapper);
                var isDeleted = await taskRepository.DeleteTaskAsync(taskId);
                if (isDeleted)
                    return Ok($"Task with ID {taskId} has been successfully deleted."); // Return success message

                return BadRequest("Unable to delete the task."); // Unexpected failure
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Task not found
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}"); // Handle unexpected errors
            }
        }




    }
}
