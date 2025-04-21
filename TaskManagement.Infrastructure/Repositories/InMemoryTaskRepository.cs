using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Infrastructure.Repositories
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly JsonDataService _dataService;
        private List<TaskItem> _tasks;

        public InMemoryTaskRepository(JsonDataService dataService)
        {
            _dataService = dataService;
            _tasks = new List<TaskItem>();
            LoadDataAsync().Wait();
        }

        private async Task LoadDataAsync()
        {
            _tasks = await _dataService.LoadTasksAsync();
        }

        private async Task SaveDataAsync()
        {
            await _dataService.SaveTasksAsync(_tasks);
        }

        public Task<TaskItem> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
        }

        public Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return Task.FromResult(_tasks.AsEnumerable());
        }

        public Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId)
        {
            return Task.FromResult(_tasks.Where(t => t.AssignedUserId == userId));
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            _tasks.Add(task);
            await SaveDataAsync();
            return task;
        }

        public async Task UpdateAsync(TaskItem task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                var index = _tasks.IndexOf(existingTask);
                _tasks[index] = task;
                await SaveDataAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
                await SaveDataAsync();
            }
        }
    }
} 