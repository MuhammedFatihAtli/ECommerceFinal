using ECommerce.Application.DTOs.CategoryDTOs;

namespace ECommerce.Application.Interfaces
{
    // ICategoryService.cs", kategori işlemleri için gerekli servis arayüzünü tanımlar.
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false);
        Task<CategoryDTO> GetByIdAsync(int id);
        Task AddAsync(CategoryCreateDTO dto);
        Task UpdateAsync(CategoryEditDTO dto);
        Task DeleteAsync(int id);
    }
}
