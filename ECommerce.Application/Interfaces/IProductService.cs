using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync(bool isTrack = true, bool ignoreFilters = false);
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task CreateProductAsync(ProductCreateDTO productCreateDto);
        Task UpdateProductAsync(int id,ProductEditDTO productDto);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(int? categoryId);
        Task<IEnumerable<ProductDTO>> GetProductsBySellerIdAsync(int sellerId);
        Task<IEnumerable<ProductDTO>> GetProductsForGuestAsync(string sessionId);

    }
}
