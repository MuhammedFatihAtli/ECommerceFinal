using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.DTOs.CustomerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
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
