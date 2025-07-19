using AutoMapper;
using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<AuthResult> LoginAsync(LoginDTO dto)
        {
            // Email ile User bul
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return AuthResult.Failure("Kullanıcı bulunamadı!");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, false, false);
            if (result.Succeeded)
                return AuthResult.Success();

            return AuthResult.Failure("Invalid login attempt.");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AuthResult> RegisterAsync(RegisterDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            user.UserName = dto.Email;
            user.Status = true;
            user.CreatedDate = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return AuthResult.Success();
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return AuthResult.Failure(errors);
        }

        public async Task<int?> GetCurrentUserIdAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            return user?.Id;
        }

        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            return await _userManager.GetUserAsync(userPrincipal);
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }

}
