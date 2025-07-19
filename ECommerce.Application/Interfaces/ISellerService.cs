using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
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
