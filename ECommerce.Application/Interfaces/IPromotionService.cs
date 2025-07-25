using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    // IPromotionService.cs", promosyon işlemleri için gerekli servis arayüzünü tanımlar.
    public interface IPromotionService
    {
        Task CreateAsync(Promotion promotion);
        Task UpdateAsync(Promotion promotion);
        Task<Promotion> GetByIdAsync(int id);
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
