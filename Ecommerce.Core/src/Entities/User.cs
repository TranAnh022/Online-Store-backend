
using Ardalis.GuardClauses;
using Ecommerce.Core.src.Entities.CartAggregate;
using Ecommerce.Core.src.Entities.OrderAggregate;
using Ecommerce.Core.src.ValueObjects;

namespace Ecommerce.Core.src.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Avatar { get; set; }
        public UserRole Role { get; set; }

        public IEnumerable<Order> Orders { get; set; }
        public Cart Cart { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

        // Parameterless constructor required for generics and serialization purposes
        public User() { }

        // Parameterized constructor for convenient instantiation
        public User(string name, string email, string password, string avatar, UserRole role)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Password = password;
            Avatar = avatar;
            Role = role;
        }

        // Method for update user name
        public void UpdateName(string newName)
        {
            Guard.Against.NullOrEmpty(newName, nameof(newName));
            Name = newName;
        }

        // Method for update user email
        public void UpdateEmail(string newEmail)
        {
            Guard.Against.NullOrEmpty(newEmail, nameof(newEmail));
            Guard.Against.InvalidInput(newEmail, nameof(newEmail), e => e.Contains('@'), "Email must contain '@'.");
            Email = newEmail;
        }

        // Method for update user password
        public void UpdatePassword(string newPassword)
        {
            Guard.Against.NullOrEmpty(newPassword, nameof(newPassword), "Password cannot be empty.");
            Password = newPassword;
        }

        // Method for update user avatar
        public void UpdateAvatar(string newAvatar)
        {
            // Check if avatar URL is well-formed only if it's provided
            if (!string.IsNullOrWhiteSpace(newAvatar))
            {
                Guard.Against.InvalidInput(newAvatar, nameof(newAvatar), uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute), "Avatar must be a valid URL.");
            }
            Avatar = newAvatar;
        }

        // Method for update user role
        public void UpdateRole(UserRole newRole)
        {
            Guard.Against.InvalidInput(newRole, nameof(newRole), role => Enum.IsDefined(typeof(UserRole), role), "Invalid role specified.");
            Role = newRole;
        }

    }
}