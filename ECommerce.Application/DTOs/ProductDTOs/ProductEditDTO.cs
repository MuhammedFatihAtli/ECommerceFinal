using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.DTOs.ProductDTOs
{
    // ProductEditDTO.cs, mevcut bir ürünü düzenlemek için gerekli bilgileri tutar.
    public class ProductEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? ImageFile { get; set; } // yeni resim yüklenirse
        public string? ImagePath { get; set; } // eski resmi göstermek için
        public int? PromotionId { get; set; }
    }
}

