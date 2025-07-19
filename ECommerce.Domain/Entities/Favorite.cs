using System;

namespace ECommerce.Domain.Entities
{
    public class Favorite
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 