using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Sistemde misafir kullanıcıları temsil eden sınıf.
    /// Kullanıcı hesabı olmadan işlem yapanlar için kullanılır.
    /// </summary>
    public class Guest : User, IBase
    {
        /// <summary>
        /// Misafirin benzersiz kimliği.
        /// </summary>
        public int GuestId { get; set; }

        /// <summary>
        /// Misafirin oluşturduğu siparişler.
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Misafirin sahip olduğu ürünler (örneğin, favoriler veya sepet ürünleri olabilir).
        /// </summary>
        public ICollection<Product> Products { get; set; }

        /// <summary>
        /// Misafir kullanıcıyı soft delete ile silinmiş olarak işaretler.
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Soft delete ile silinmiş misafir kullanıcıyı geri yükler.
        /// </summary>
        public void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.Now;
        }
    }
}

