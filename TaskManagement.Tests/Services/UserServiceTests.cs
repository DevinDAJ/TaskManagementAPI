using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateUser_WithValidData_ShouldCreateUser()
        {
            var createUserDto = new CreateUserDto
            {
                Username = "testuser",
                Email = "test@example.com"
            };

            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);

            var result = await _userService.CreateUserAsync(createUserDto);

            Assert.NotNull(result);
            Assert.Equal(createUserDto.Username, result.Username);
            Assert.Equal(createUserDto.Email, result.Email);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.True(result.CreatedAt > DateTime.MinValue);
        }

        [Fact]
        public async Task UpdateUser_WithValidData_ShouldUpdateUser()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                Username = "originaluser",
                Email = "original@example.com",
                CreatedAt = DateTime.UtcNow,
                AssignedTasks = new List<TaskItem>()
            };

            var updateUserDto = new UpdateUserDto
            {
                Username = "updateduser",
                Email = "updated@example.com"
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            var result = await _userService.UpdateUserAsync(userId, updateUserDto);

            Assert.NotNull(result);
            Assert.Equal(updateUserDto.Username, result.Username);
            Assert.Equal(updateUserDto.Email, result.Email);
            Assert.Equal(userId, result.Id);
            Assert.True(result.UpdatedAt > DateTime.MinValue);
        }

        [Fact]
        public async Task UpdateUser_WithNonExistentUser_ShouldThrowArgumentException()
        {
            var userId = Guid.NewGuid();
            var updateUserDto = new UpdateUserDto
            {
                Username = "testuser",
                Email = "test@example.com"
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _userService.UpdateUserAsync(userId, updateUserDto));
        }

        [Fact]
        public async Task DeleteUser_WithExistingUser_ShouldDeleteUser()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                Username = "testuser",
                Email = "test@example.com",
                CreatedAt = DateTime.UtcNow,
                AssignedTasks = new List<TaskItem>()
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(existingUser);

            _userRepositoryMock.Setup(r => r.DeleteAsync(userId))
                .Returns(Task.CompletedTask);

            await _userService.DeleteUserAsync(userId);

            _userRepositoryMock.Verify(r => r.DeleteAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_WithNonExistentUser_ShouldThrowArgumentException()
        {
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _userService.DeleteUserAsync(userId));
        }

        [Fact]
        public async Task GetUserById_WithExistingUser_ShouldReturnUser()
        {
            var userId = Guid.NewGuid();
            var existingUser = new User
            {
                Id = userId,
                Username = "testuser",
                Email = "test@example.com",
                CreatedAt = DateTime.UtcNow,
                AssignedTasks = new List<TaskItem>()
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(existingUser);

            var result = await _userService.GetUserByIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(existingUser.Username, result.Username);
            Assert.Equal(existingUser.Email, result.Email);
        }

        [Fact]
        public async Task GetUserById_WithNonExistentUser_ShouldReturnNull()
        {
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            var result = await _userService.GetUserByIdAsync(userId);

            Assert.Null(result);
        }
    }
} 