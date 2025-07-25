using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.DTOs.ProductDTOs
{
    // ProductCreateDTO.cs, yeni bir ürün oluşturmak için gerekli bilgileri tutar.
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla {1} karakter olabilir")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; }
        
        [StringLength(500, ErrorMessage = "Açıklama en fazla {1} karakter olabilir")]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Stok miktarı zorunludur")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya daha büyük olmalıdır")]
        [Display(Name = "Stok")]
        public int Stock { get; set; }
        
        [Required(ErrorMessage = "Ürün resmi zorunludur")]
        [Display(Name = "Ürün Resmi")]
        public IFormFile ImageFile { get; set; }
        
        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
        public int SellerId { get; set; }
    }
}
