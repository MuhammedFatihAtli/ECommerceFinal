using ECommerce.Application.DTOs.UserDTOs;

namespace ECommerce.Application.Interfaces
{
    // IUserService.cs, kullanıcı işlemleri için gerekli metotları tanımlar.
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
