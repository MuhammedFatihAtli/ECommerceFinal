using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    // BaseDTO.cs", temel veri transfer nesnesi (DTO) sınıfıdır.
    public record BaseDTO
    {
        public int Id { get; set; }
    }
}
