using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Services;

namespace TaskManagement.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly JsonDataService _dataService;
        private List<User> _users;

        public InMemoryUserRepository(JsonDataService dataService)
        {
            _dataService = dataService;
            _users = new List<User>();
            LoadDataAsync().Wait();
        }

        private async Task LoadDataAsync()
        {
            _users = await _dataService.LoadUsersAsync();
        }

        private async Task SaveDataAsync()
        {
            await _dataService.SaveUsersAsync(_users);
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public async Task<User> AddAsync(User user)
        {
            _users.Add(user);
            await SaveDataAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                var index = _users.IndexOf(existingUser);
                _users[index] = user;
                await SaveDataAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
                await SaveDataAsync();
            }
        }
    }
} 