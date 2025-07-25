using ECommerce.Domain.Commons;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Sistemdeki müşteri kullanıcıyı temsil eder. 
    /// Temel kullanıcı bilgileri dışında adres, telefon ve sipariş bilgilerini içerir.
    /// </summary>
    public class Customer : User, IBase
    {
        /// <summary>
        /// Müşterinin açık adres bilgisi.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Müşterinin telefon numarası.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Müşteriye ait profil resmi URL’si (opsiyonel).
        /// </summary>
        public string? ProfileImageUrl { get; set; }

        /// <summary>
        /// Müşterinin geçmiş siparişleri.
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Oluşturulma tarihi (IBase arayüzü).
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Son güncellenme tarihi (IBase arayüzü).
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Soft delete bayrağı (IBase arayüzü).
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Soft delete işlemi: Müşteriyi silinmiş olarak işaretler.
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Silinmiş müşteriyi geri aktif hale getirir.
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.Now;
        }
    }
}


