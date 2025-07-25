using System;

namespace ECommerce.Domain.Commons
{
    /// <summary>
    /// Tüm varlık sınıfları için ortak özellikleri barındıran soyut temel sınıftır.
    /// </summary>
    public abstract class BaseEntity : IBase
    {
        /// <summary>
        /// Varlığın benzersiz kimliğidir.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Varlığın oluşturulduğu tarihi belirtir.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Varlığın en son güncellendiği tarihi belirtir.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Varlığın silinip silinmediğini belirten işaretçidir (soft delete).
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Soft delete işlemiyle silinmiş bir varlığı geri yükler (Restore).
        /// </summary>
        public void Restore()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                UpdatedDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Varlığı veritabanından fiziksel olarak silmeden, işaretleyerek siler (soft delete).
        /// </summary>
        public void SoftDelete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                UpdatedDate = DateTime.Now;
            }
        }
    }
}
