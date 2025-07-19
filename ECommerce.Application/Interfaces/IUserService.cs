using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync(bool isTrack = true);
        Task<IEnumerable<UserDTO>> GetAllDeletedAsync();
        Task<UserDTO?> GetByIdAsync(int id, bool ignoreFilters = false);
        Task AddAsync(UserCreateDTO dto);
        Task UpdateAsync(UserUpdateDTO dto);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
        Task RestoreAsync(int id);

    }
}
