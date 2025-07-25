using ECommerce.Domain.Commons;
using System;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Bir ürüne yapılmış kullanıcı yorumunu temsil eder.
    /// </summary>
    public class Comment : IBase
    {
        /// <summary>
        /// Yorumun benzersiz kimliği.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Yorumun metni.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Ürüne verilen puan (1 ile 5 arasında).
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Yorumun oluşturulduğu tarih.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Yorumu yapan kullanıcının görünen adı.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Yorumun yapıldığı ürünün ID'si (foreign key).
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Yorumun ait olduğu ürün.
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Yorumu yapan kullanıcının ID'si (opsiyonel).
        /// Null ise anonim kullanıcı anlamına gelir.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Yorumu yapan kullanıcı nesnesi.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Yorumun en son güncellendiği tarih.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Yorumun silinip silinmediğini belirten bayrak (soft delete için).
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// IBase arayüzünden gelen oluşturulma tarihi.
        /// Genelde CreatedAt ile aynıdır.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Soft delete işlemiyle yorumu siler (veritabanından değil, işaretlenerek).
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Soft delete ile silinmiş yorumu geri yükler.
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.Now;
        }
    }
}
