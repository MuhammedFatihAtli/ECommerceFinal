using ECommerce.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.VMs.Promotion
{
    // PromotionCreateVM, yeni promosyon oluşturma işlemleri için kullanılan ViewModel sınıfıdır.
    public class PromotionCreateVM
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "İndirim oranı 1 ile 100 arasında olmalıdır.")]
        public int DiscountRate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Başlangıç tarihi zorunludur.")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Lütfen promosyonun uygulanacağı ürünü seçiniz.")]
        public int SelectedProductId { get; set; }

        // BU SATIR OLMALI:
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}

