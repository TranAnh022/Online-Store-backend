using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.DTO;
using Ecommerce.Service.src.Service;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Service
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
        private readonly Mock<ICartRepository> _mockCartRepository = new Mock<ICartRepository>();

        public UserServiceTests()
        {
            _userService = new UserService(_mockUserRepository.Object, _mockMapper.Object, _mockPasswordHasher.Object, _mockCartRepository.Object);
        }

        [Fact]
        public async Task CreateOneAsync_ShouldCreateUser_WhenValidInput()
        {
            // Arrange
            var createUserDto = new UserCreateDto { Password = "password123", Name = "John Doe", Email = "john@example.com" };
            var createdUser = new User { Id = Guid.NewGuid(), Name = "John Doe", Email = "john@example.com", Password = "hashedPassword" };

            _mockMapper.Setup(m => m.Map<User>(It.IsAny<UserCreateDto>())).Returns(createdUser);
            _mockPasswordHasher.Setup(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashedPassword");
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(createdUser);
            _mockMapper.Setup(m => m.Map<UserReadDto>(It.IsAny<User>())).Returns(new UserReadDto { Name = "John Doe", Email = "john@example.com" });

            // Act
            var result = await _userService.CreateOneAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task SearchUsersAsync_ReturnsFilteredData_WhenCalledWithQueryOptions()
        {
            // Arrange
            var users = new List<User> { new User { Name = "John" }, new User { Name = "Jane" } };
            var userReadDtos = new List<UserReadDto> { new UserReadDto { Name = "John" }, new UserReadDto { Name = "Jane" } };
            var options = new UserQueryOptions { Search = "John", Page = 1, PageSize = 2, SortBy = "Name", SortOrder = "asc" };
            _mockUserRepository.Setup(r => r.ListAsync(It.IsAny<UserQueryOptions>())).ReturnsAsync(users);
            _mockMapper.Setup(m => m.Map<IEnumerable<UserReadDto>>(It.IsAny<IEnumerable<User>>())).Returns(userReadDtos);

            // Act
            var result = await _userService.SearchUsersAsync(options);

            // Assert
            Assert.Equal(2, result.Count());
            _mockUserRepository.Verify(r => r.ListAsync(It.IsAny<UserQueryOptions>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOneAsync_UpdatesOnlyProvidedFields_WhenCalledWithPartialData()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var existingUser = new User { Id = userId, Name = "Original Name", Email = "original@example.com" };
            var updateDto = new UserUpdateDto { Name = "Updated Name" }; // Only update name
            _mockUserRepository.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(existingUser);
            _mockMapper.Setup(m => m.Map(It.IsAny<UserUpdateDto>(), It.IsAny<User>()))
                       .Callback<UserUpdateDto, User>((dto, user) => user.Name = dto.Name ?? user.Name);
            _mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(existingUser);
            _mockMapper.Setup(m => m.Map<UserReadDto>(It.IsAny<User>())).Returns(new UserReadDto { Name = "Updated Name" });

            // Act
            var result = await _userService.UpdateOneAsync(userId, updateDto);

            // Assert
            Assert.Equal("Updated Name", result.Name);
            _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOneAsync_ShouldThrowKeyNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var updateDto = new UserUpdateDto { Name = "Updated Name" };
            Guid userId = Guid.NewGuid();

            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User)null!);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateOneAsync(userId, updateDto));
        }

        [Fact]
        public async Task UpdatePasswordAsync_ShouldUpdatePassword_WhenUserExists()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            string newPassword = "newSecurePassword123";
            var existingUser = new User { Id = userId, Password = "oldPassword" };

            _mockUserRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existingUser);
            _mockPasswordHasher.Setup(h => h.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashedNewPassword");
            _mockUserRepository.Setup(r => r.UpdateAsync(It.IsAny<User>())).ReturnsAsync(existingUser);

            // Act
            var result = await _userService.UpdatePasswordAsync(userId, newPassword);

            // Assert
            Assert.True(result);
            _mockUserRepository.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [InlineData(true, true, null)] // Test case for successful deletion
        [InlineData(false, false, null)] // Test case for failed deletion (user not found)
        [InlineData(false, true, typeof(InvalidOperationException))] // Test case for exception handling
        public async Task DeleteOneAsync_HandlesVariousScenarios_Correctly(bool expectedResult, bool repoSetupResult, Type? exceptionType)
        {
            // Arrange
            var userId = Guid.NewGuid();
            if (exceptionType == null)
            {
                _mockUserRepository.Setup(r => r.DeleteAsync(userId)).ReturnsAsync(repoSetupResult);
            }
            else
            {
                _mockUserRepository.Setup(r => r.DeleteAsync(userId)).ThrowsAsync((Exception)Activator.CreateInstance(exceptionType, "Simulated database error")!);
            }

            // Act & Assert
            if (exceptionType == null)
            {
                var result = await _userService.DeleteOneAsync(userId);
                Assert.Equal(expectedResult, result);
            }
            else
            {
                await Assert.ThrowsAsync(exceptionType, () => _userService.DeleteOneAsync(userId));
            }

            _mockUserRepository.Verify(r => r.DeleteAsync(userId), Times.Once);
        }
    }
}
