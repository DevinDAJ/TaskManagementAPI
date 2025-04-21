using System;

namespace TaskManagement.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
    }

    public class UpdateUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
    }
} 