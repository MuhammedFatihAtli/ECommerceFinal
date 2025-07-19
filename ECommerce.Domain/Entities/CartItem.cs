using ECommerce.Domain.Commons;

namespace ECommerce.Domain.Entities
{
    public class CartItem : IBase
    {
        public int Id { get; set; }

        // Sepet sahibi kullanıcı
        public int UserId { get; set; }
        public virtual User User { get; set; }

        // Sepete eklenen ürün
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string? GuestId { get; set; }

        // Adet bilgisi
        public int Quantity { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Status { get; set; } = true;

        public void IncreaseQuantity(int count = 1)
        {
            Quantity += count;
        }

        public void DecreaseQuantity(int count = 1)
        {
            Quantity = Math.Max(1, Quantity - count);
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}

