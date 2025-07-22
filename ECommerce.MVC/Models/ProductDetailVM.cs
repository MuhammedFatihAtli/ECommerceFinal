using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.ProductDTOs;

namespace ECommerce.MVC.Models
{
    public class ProductDetailVM
    {
        public ProductDTO Product { get; set; }
        public List<CommentDTO> Comments { get; set; }
        public CommentDTO NewComment { get; set; } = new CommentDTO();
    }
}
