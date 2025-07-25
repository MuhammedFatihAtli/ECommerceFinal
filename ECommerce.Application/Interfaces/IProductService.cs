using ECommerce.Application.DTOs.ProductDTOs;

namespace ECommerce.Application.Interfaces
{
    // IProductService.cs, ürünlerle ilgili işlemleri tanımlar.
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
