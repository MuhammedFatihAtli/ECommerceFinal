using ECommerce.Application.DTOs.ProductDTOs;

namespace ECommerce.Application.VMs.Promotion
{
    // PromotionVM, promosyon işlemleri için kullanılan ViewModel sınıfıdır.
    public class PromotionVM
    {
        public List<ProductDTO> Products { get; set; } = new();
        public List<Domain.Entities.Promotion> Promotions { get; set; } = new();
    }
}

