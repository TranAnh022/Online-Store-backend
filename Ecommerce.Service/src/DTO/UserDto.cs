using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.ValueObjects;
using Ecommerce.Service.src.DTO;


namespace Ecommerce.Service.DTO
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string Avatar { get; set; }

        public void Transform(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            Role = user.Role;
            Avatar = user.Avatar;
        }
    }

    public class UserUpdateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public UserRole Role { get; set; }

    }

    public class UserCreateDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Avatar { get; set; }
        public UserRole Role { get; set; }
    }

    public class UserRoleUpdateDto
    {
        public UserRole NewRole { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public CartReadDto Basket { get; set; }
    }
}

