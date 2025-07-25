using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.DTOs.CustomerDTOs;

namespace ECommerce.Application.Interfaces
{
    // ICustomerService.cs, müşteri işlemleri için gerekli metotları tanımlar.
    public interface ICustomerService
    {
        Task<List<CustomerListDTO>> GetAllAsync();
        Task<CustomerDetailDTO> GetByIdAsync(int id);
        Task AddAsync(CustomerCreateDTO dto);
        Task UpdateAsync(CustomerUpdateDTO dto);
        Task DeleteAsync(int id);
        Task<LoginResultDTO> LoginAsync(LoginDTO dto);
        Task<CustomerDetailDTO> GetByUserNameAsync(string userName);
    }
}
