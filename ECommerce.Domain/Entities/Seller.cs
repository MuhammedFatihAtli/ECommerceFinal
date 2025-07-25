using ECommerce.Domain.Commons;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Sistemde ürün satan satıcıyı temsil eden sınıf.
    /// User sınıfından türemiştir, kullanıcı kimlik bilgilerini içerir.
    /// </summary>
    public class Seller : User, IBase
    {
        /// <summary>
        /// Satıcının benzersiz ID'si.
        /// </summary>
        public int SellerId { get; set; }

        /// <summary>
        /// Satıcıya ait şirket/işletme adı.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Satıcı logosunun URL'si.
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Satıcının fiziksel adresi.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Satıcıya ait ürünler koleksiyonu.
        /// </summary>
        public ICollection<Product> Products { get; set; }

        /// <summary>
        /// Satıcıya ait siparişler koleksiyonu.
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Satıcıyı soft delete (silinmiş) olarak işaretler.
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Soft delete ile işaretlenmiş satıcıyı aktif hale getirir.
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.Now;
        }
    }
}

