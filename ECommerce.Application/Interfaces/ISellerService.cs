using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.DTOs.SellerDTOs;

namespace ECommerce.Application.Interfaces
{
    // ISellerService.cs, satıcı işlemleri için gerekli servis arayüzünü tanımlar.
    public interface ISellerService
    {
        Task<List<SellerDTO>> GetAllAsync();
        Task<SellerDTO> GetByIdAsync(int id);
        Task AddAsync(SellerCreateDTO dto);
        Task UpdateAsync(SellerUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<LoginResultDTO> LoginAsync(LoginDTO dto);
        Task<SellerDTO> GetByUserNameAsync(string userName);
    }
}
