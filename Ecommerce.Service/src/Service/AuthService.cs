using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entities;
using Ecommerce.Core.src.Interfaces;
using Ecommerce.Service.src.ServiceAbstract;
using Ecommerce.Service.src.Shared;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Service.src.Service
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly PasswordService _passwordService;

        public AuthService(IUserRepository userRepo, ITokenService tokenService
        , PasswordService passwordService)
        {

            _userRepo = userRepo;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<User> AuthenticateUserAsync(string token)
        {
            var userId = _tokenService.VerifyToken(token);
            var user = await _userRepo.GetByIdAsync(userId);
            return user;
        }


        public async Task<string> LogInAsync(UserCredential userCredential)
        {
            var foundByEmail = await _userRepo.GetByEmailAsync(userCredential.Email);
            if (foundByEmail == null)
            {
                throw new Exception("Email not found");
            }
            var isPasswordMatch = _passwordService.VerifyPassword(foundByEmail, foundByEmail.Password, userCredential.Password);
            if (isPasswordMatch == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }
            return _tokenService.GetToken(foundByEmail);
        }

    }
}