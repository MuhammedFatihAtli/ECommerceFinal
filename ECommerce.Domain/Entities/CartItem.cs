using ECommerce.Domain.Commons;
using System;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Sepetteki her bir ürün öğesini temsil eder. Hem kullanıcıya hem de misafire ait olabilir.
    /// </summary>
    public class CartItem : IBase
    {
        /// <summary>
        /// Sepet öğesinin benzersiz kimliği.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Sepetin ait olduğu kullanıcının ID'si (null ise misafir kullanıcıdır).
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Sepetin ait olduğu kullanıcı (authenticated user).
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Sepete eklenen ürünün ID'si.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Sepete eklenen ürün.
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Sepetin ait olduğu misafir kullanıcının session ID'si (user yoksa kullanılır).
        /// </summary>
        public string? GuestId { get; set; }

        /// <summary>
        /// Üründen sepete kaç adet eklendiği.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Sepet öğesinin oluşturulma tarihi.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Sepet öğesinin son güncellenme tarihi.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Soft delete özelliği için silinip silinmediğini belirtir.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Sepet öğesinin aktif olup olmadığını gösterir.
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// Sepetteki ürün adedini artırır.
        /// </summary>
        /// <param name="count">Artırılacak miktar (varsayılan: 1).</param>
        public void IncreaseQuantity(int count = 1)
        {
            Quantity += count;
        }

        /// <summary>
        /// Sepetteki ürün adedini azaltır (1'in altına düşmez).
        /// </summary>
        /// <param name="count">Azaltılacak miktar (varsayılan: 1).</param>
        public void DecreaseQuantity(int count = 1)
        {
            Quantity = Math.Max(1, Quantity - count);
        }

        /// <summary>
        /// Sepet öğesini soft delete ile işaretler.
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Soft delete edilen sepet öğesini geri yükler.
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.UtcNow;
        }
    }
}


