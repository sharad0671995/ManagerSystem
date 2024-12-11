
namespace EducationSystem.Entity.DTO.User
{
    public class AuthResponseDto
    {
        public string? Token { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public string? RefreshToken { get; set; } // Add this
    }
}