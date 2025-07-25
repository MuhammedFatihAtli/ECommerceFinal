using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.BasketDTOs
{
    //CartDTO.cs", sepetin içeriğini ve toplam fiyatını tutar.
    public class CartDTO
    {
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    }
}
