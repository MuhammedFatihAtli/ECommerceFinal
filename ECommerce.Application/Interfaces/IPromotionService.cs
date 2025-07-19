using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IPromotionService
    {
        Task CreateAsync(Promotion promotion);
        Task UpdateAsync(Promotion promotion);
        Task<Promotion> GetByIdAsync(int id);
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
