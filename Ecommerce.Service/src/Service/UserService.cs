using AutoMapper;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.DTO;
using Ecommerce.Service.src.ServiceAbstract;
using Microsoft.AspNetCore.Identity;


namespace Ecommerce.Service.src.Service
{
    public class UserService : BaseService<User, UserReadDto, UserCreateDto, UserUpdateDto, UserQueryOptions>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher, ICartRepository cartRepository)
            : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _cartRepository = cartRepository;
        }

        public override async Task<UserReadDto> CreateOneAsync(UserCreateDto createDto)
        {
            // Create the User entity
            var user = new User(
                email: createDto.Email,
                name: createDto.Name,
                avatar: createDto.Avatar!,
                role: createDto.Role,
                password: createDto.Password
            );

            user.Password = _passwordHasher.HashPassword(user, createDto.Password);
            // Add the user to the repository
            user = await _userRepository.AddAsync(user);
            // Create a cart for the new user
            await _cartRepository.CreateCartForUser(user.Id);
            // Map and return the result
            return _mapper.Map<UserReadDto>(user);
        }

        public override async Task<bool> DeleteOneAsync(Guid id)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(id);
            if (cart != null)
            {
                var cartDeleted = await _cartRepository.DeleteAsync(cart.Id);
                if (!cartDeleted)
                {
                    return false;
                }
            }
            var userDeleted = await _userRepository.DeleteAsync(id);
            return userDeleted;
        }

        public async Task<IEnumerable<UserReadDto>> SearchUsersAsync(UserQueryOptions options)
        {
            var users = await _userRepository.ListAsync(options);
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<bool> UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.Password = _passwordHasher.HashPassword(user, newPassword);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<UserReadDto> UpdateRoleAsync(Guid userId, UserRoleUpdateDto roleUpdateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId) ?? throw new KeyNotFoundException("User not found");
            user.Role = roleUpdateDto.NewRole;
            user = await _userRepository.UpdateAsync(user);
            return _mapper.Map<UserReadDto>(user);
        }
    }
}