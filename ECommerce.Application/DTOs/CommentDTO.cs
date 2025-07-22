using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Yorum metni boş bırakılamaz.")]
        public string Text { get; set; }  // Yorum içeriği için
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }

}
