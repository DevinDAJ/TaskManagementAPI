using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Title { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        [Required]
        public TaskPriority Priority { get; set; }
        
        [Required]
        public TaskItemStatus Status { get; set; }
        
        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public enum TaskItemStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
} 