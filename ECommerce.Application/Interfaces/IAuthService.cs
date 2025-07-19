using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginDTO dto);
        Task LogoutAsync();
        Task<AuthResult> RegisterAsync(RegisterDTO dto);
        Task<int?> GetCurrentUserIdAsync(ClaimsPrincipal user);
        Task<User> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<IList<string>> GetUserRolesAsync(User user);
    }
}
