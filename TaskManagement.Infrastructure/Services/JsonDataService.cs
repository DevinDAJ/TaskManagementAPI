using System.Text.Json;
using TaskManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Infrastructure.Services
{
    public class JsonDataService
    {
        private readonly string _dataDirectory;
        private readonly string _usersFilePath;
        private readonly string _tasksFilePath;
        private readonly ILogger<JsonDataService> _logger;

        public JsonDataService(ILogger<JsonDataService> logger)
        {
            _logger = logger;
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _usersFilePath = Path.Combine(_dataDirectory, "users.json");
            _tasksFilePath = Path.Combine(_dataDirectory, "tasks.json");

            try
            {
                if (!Directory.Exists(_dataDirectory))
                {
                    _logger.LogInformation("Creating data directory at {DataDirectory}", _dataDirectory);
                    Directory.CreateDirectory(_dataDirectory);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating data directory at {DataDirectory}", _dataDirectory);
                throw;
            }
        }

        public async Task<List<User>> LoadUsersAsync()
        {
            try
            {
                if (!File.Exists(_usersFilePath))
                {
                    _logger.LogInformation("Users file does not exist at {UsersFilePath}, returning empty list", _usersFilePath);
                    return new List<User>();
                }

                var json = await File.ReadAllTextAsync(_usersFilePath);
                var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                _logger.LogInformation("Loaded {Count} users from {UsersFilePath}", users.Count, _usersFilePath);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users from {UsersFilePath}", _usersFilePath);
                return new List<User>();
            }
        }

        public async Task SaveUsersAsync(List<User> users)
        {
            try
            {
                var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_usersFilePath, json);
                _logger.LogInformation("Saved {Count} users to {UsersFilePath}", users.Count, _usersFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving users to {UsersFilePath}", _usersFilePath);
                throw;
            }
        }

        public async Task<List<TaskItem>> LoadTasksAsync()
        {
            try
            {
                if (!File.Exists(_tasksFilePath))
                {
                    _logger.LogInformation("Tasks file does not exist at {TasksFilePath}, returning empty list", _tasksFilePath);
                    return new List<TaskItem>();
                }

                var json = await File.ReadAllTextAsync(_tasksFilePath);
                var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
                _logger.LogInformation("Loaded {Count} tasks from {TasksFilePath}", tasks.Count, _tasksFilePath);
                return tasks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tasks from {TasksFilePath}", _tasksFilePath);
                return new List<TaskItem>();
            }
        }

        public async Task SaveTasksAsync(List<TaskItem> tasks)
        {
            try
            {
                var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_tasksFilePath, json);
                _logger.LogInformation("Saved {Count} tasks to {TasksFilePath}", tasks.Count, _tasksFilePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving tasks to {TasksFilePath}", _tasksFilePath);
                throw;
            }
        }
    }
} 