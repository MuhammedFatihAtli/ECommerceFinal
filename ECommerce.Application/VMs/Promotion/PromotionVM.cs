using ECommerce.Application.DTOs.ProductDTOs;

namespace ECommerce.Application.VMs.Promotion
{
    public class PromotionVM
    {
        public List<ProductDTO> Products { get; set; } = new();
        public List<Domain.Entities.Promotion> Promotions { get; set; } = new();
    }
}

