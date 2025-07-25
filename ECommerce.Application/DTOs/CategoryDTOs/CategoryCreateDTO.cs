using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.CategoryDTOs
{
    // CategoryCreateDTO.cs, yeni bir kategori oluşturmak için gerekli bilgileri tutar.
    public record CategoryCreateDTO
    {
        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla {1} karakter olabilir")]
        [Display(Name = "Kategori Adı")]
        public string Name { get; init; }
        
        [StringLength(500, ErrorMessage = "Açıklama en fazla {1} karakter olabilir")]
        [Display(Name = "Açıklama")]
        public string? Description { get; init; }
    }
}
