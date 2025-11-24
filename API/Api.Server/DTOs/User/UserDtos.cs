namespace API.Api.Server.DTOs.User
{
    // POST /api/users ke liye
    public class CreateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public int Role { get; set; }   // UserRole enum ka int (0=Admin,1=Manager,2=Employee)
    }

    // PUT /api/users/{id} ke liye
    public class UpdateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; }
    }

    // Response me jo bhejna hai
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // enum ka naam string me
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
