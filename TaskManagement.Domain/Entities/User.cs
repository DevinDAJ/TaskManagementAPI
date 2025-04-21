using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public required string Username { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Email { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        public required ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
} 