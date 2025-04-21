using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            if (task.AssignedUserId.HasValue)
            {
                task.AssignedUser = await _userRepository.GetByIdAsync(task.AssignedUserId.Value);
            }

            return MapToDto(task);
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            var tasksList = tasks.ToList();

            foreach (var task in tasksList.Where(t => t.AssignedUserId.HasValue))
            {
                task.AssignedUser = await _userRepository.GetByIdAsync(task.AssignedUserId.Value);
            }

            return tasksList.Select(MapToDto);
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByUserIdAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
            var tasksList = tasks.ToList();
            var user = await _userRepository.GetByIdAsync(userId);

            foreach (var task in tasksList)
            {
                task.AssignedUser = user;
            }

            return tasksList.Select(MapToDto);
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            ValidateDueDate(createTaskDto.DueDate);

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                DueDate = createTaskDto.DueDate,
                Priority = createTaskDto.Priority,
                Status = TaskItemStatus.Pending,
                AssignedUserId = createTaskDto.AssignedUserId,
                CreatedAt = DateTime.UtcNow
            };

            if (task.AssignedUserId.HasValue)
            {
                task.AssignedUser = await _userRepository.GetByIdAsync(task.AssignedUserId.Value);
                if (task.AssignedUser == null)
                {
                    throw new ArgumentException("Assigned user not found");
                }
            }

            await _taskRepository.AddAsync(task);
            return MapToDto(task);
        }

        public async Task<TaskDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            ValidateDueDate(updateTaskDto.DueDate);

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.DueDate = updateTaskDto.DueDate;
            task.Priority = updateTaskDto.Priority;
            task.Status = updateTaskDto.Status;
            task.UpdatedAt = DateTime.UtcNow;

            if (updateTaskDto.AssignedUserId.HasValue)
            {
                task.AssignedUser = await _userRepository.GetByIdAsync(updateTaskDto.AssignedUserId.Value);
                if (task.AssignedUser == null)
                {
                    throw new ArgumentException("Assigned user not found");
                }
                task.AssignedUserId = updateTaskDto.AssignedUserId;
            }

            await _taskRepository.UpdateAsync(task);
            return MapToDto(task);
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new ArgumentException("Task not found");
            }

            await _taskRepository.DeleteAsync(id);
        }

        private static void ValidateDueDate(DateTime dueDate)
        {
            if (dueDate < DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Due date cannot be in the past");
            }
        }

        private static TaskDto MapToDto(TaskItem task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                AssignedUserId = task.AssignedUserId,
                AssignedUserName = task.AssignedUser?.Username,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
} 