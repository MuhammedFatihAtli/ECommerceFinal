using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Comment : IBase
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int Rating { get; set; } // 1-5 arası puan

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserName { get; set; }

        // Foreign keys
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int? UserId { get; set; } // Anonymous yorumlara da izin verebilirsin
        public User User { get; set; }
        
        public DateTime? UpdatedDate { get ; set ; }
        public bool IsDeleted { get ; set ; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;

        public void Restore()
        {
            throw new NotImplementedException();
        }

        public void SoftDelete()
        {
            throw new NotImplementedException();
        }
    }
}

