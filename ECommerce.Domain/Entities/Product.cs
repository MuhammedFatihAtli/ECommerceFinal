using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImagePath { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int SellerId { get; set; }
        public virtual Seller Seller { get; set; }

        public int? GuestId { get; set; }
        public virtual Guest? Guest { get; set; }

        public int? PromotionId { get; set; }
        public virtual Promotion? Promotion { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
