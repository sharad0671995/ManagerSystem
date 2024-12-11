using System.ComponentModel.DataAnnotations;

namespace EducationSystem.Entity.DTO.User
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role Name is required.")]
        public string RoleName { get; set; } = null!;
    }
}