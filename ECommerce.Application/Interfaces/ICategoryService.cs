using ECommerce.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false);
        Task<CategoryDTO> GetByIdAsync(int id);
        Task AddAsync(CategoryCreateDTO dto);
        Task UpdateAsync(CategoryEditDTO dto);
        Task DeleteAsync(int id);
    }
}
