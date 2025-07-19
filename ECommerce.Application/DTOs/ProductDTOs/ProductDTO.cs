using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.DTOs.ProductDTOs
{
    public record ProductDTO : BaseDTO
    {
        public ProductDTO(string name)
        {
            Name = name;
        }
         public string Name { get; set; }

        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }//mevcut resim yolu için
        public IFormFile? ImageFile { get; set; }// yeni resim yüklemek için
        public string CategoryName { get; set; }  // Include ile çekmek için

        public int CategoryId { get; set; } // Foreign key for Category

        public string ImagePath { get; set; } // Ürün görseli yolu
        public int? SellerId { get; set; }
        public int? PromotionId { get; set; }

    }
   
    }

