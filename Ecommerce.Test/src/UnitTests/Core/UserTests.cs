using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Ecommerce.Test.src.UnitTests.Core
{
    public class UserTests
    {
        [Fact]
        public void UpdateName_WithValidName_ShouldUpdateName()
        {
            // Arrange
            var user = new User("Initial Name", "user@example.com", "Password123", "https://example.com/avatar.jpg", UserRole.User);

            // Act
            user.UpdateName("Updated Name");

            // Assert
            user.Name.Should().Be("Updated Name");
        }

        [Fact]
        public void UpdateEmail_WithInvalidEmail_ShouldThrowException()
        {
            // Arrange
            var user = new User("User Name", "user@example.com", "Password123", "https://example.com/avatar.jpg", UserRole.User);

            // Act
            Action act = () => user.UpdateEmail("invalidemail");

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Email must contain '@'. (Parameter 'newEmail')");
        }

        [Fact]
        public void UpdatePassword_WithEmptyPassword_ShouldThrowException()
        {
            // Arrange
            var user = new User("User Name", "user@example.com", "Password123", "https://example.com/avatar.jpg", UserRole.User);

            // Act
            Action act = () => user.UpdatePassword("");

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Password cannot be empty. (Parameter 'newPassword')");
        }

        [Fact]
        public void UpdateAvatar_WithInvalidUrl_ShouldThrowException()
        {
            // Arrange
            var user = new User("User Name", "user@example.com", "Password123", "https://example.com/avatar.jpg", UserRole.User);

            // Act
            Action act = () => user.UpdateAvatar("invalid-url");

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Avatar must be a valid URL. (Parameter 'newAvatar')");
        }

        [Fact]
        public void UpdateRole_WithValidRole_ShouldUpdateRole()
        {
            // Arrange
            var user = new User("User Name", "user@example.com", "Password123", "https://example.com/avatar.jpg", UserRole.Admin);

            // Act
            user.UpdateRole(UserRole.User);

            // Assert
            user.Role.Should().Be(UserRole.User);
        }

        [Fact]
        public void UpdateRole_WithUndefinedRole_ShouldThrowException()
        {
            // Arrange
            var user = new User("User Name", "user@example.com", "Password123", "https://example.com/avatar.jpg", UserRole.Admin);

            // Act
            Action act = () => user.UpdateRole((UserRole)123);

            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Invalid role specified. (Parameter 'newRole')");
        }
    }
}