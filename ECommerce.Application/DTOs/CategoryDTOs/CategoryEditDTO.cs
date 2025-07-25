using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.CategoryDTOs
{
    // CategoryEditDTO.cs", kategori düzenleme işlemleri için gerekli bilgileri tutar.
    public record CategoryEditDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
