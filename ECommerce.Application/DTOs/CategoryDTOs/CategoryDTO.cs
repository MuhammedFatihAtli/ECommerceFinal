using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.CategoryDTOs
{
    // CategoryDTO.cs", kategori bilgilerini tutar.
    public record CategoryDTO : BaseDTO
    {
        public CategoryDTO(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
