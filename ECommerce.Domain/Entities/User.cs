using ECommerce.Domain.Commons;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Uygulamanın temel kullanıcı sınıfı. ASP.NET Identity'nin IdentityUser sınıfından türetilmiştir.
    /// </summary>
    public class User : IdentityUser<int>, IBase
    {
        /// <summary>
        /// Kullanıcının tam adı ile ve e-posta adresi ile oluşturur.
        /// </summary>
        /// <param name="fullName">Kullanıcının tam adı.</param>
        /// <param name="userEmail">Kullanıcının e-posta adresi.</param>
        public User(string fullName, string userEmail) : base(userEmail)
        {
            FullName = fullName;
            Email = userEmail;
            UserName = userEmail;
        }

        /// <summary>
        /// Parametresiz kurucu (ORM ve Identity için).
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Kullanıcının tam adı.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Kullanıcının kayıt olduğu kurs/kayitlar.
        /// </summary>
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        /// <summary>
        /// Kullanıcının oluşturulma tarihi.
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Kullanıcı bilgilerinin son güncellenme tarihi.
        /// </summary>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Kullanıcının soft delete (silinmiş) olup olmadığını gösterir.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Kullanıcının aktiflik durumu.
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// Kullanıcı şifresi (şifreleme katmanı için genellikle kullanılmaz, Identity kullanılır).
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Kullanıcının ilk adı.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Kullanıcının soyadı.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Kullanıcının yaptığı yorumlar.
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// Kullanıcının soft delete durumunu geri alır (aktif yapar).
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
        /// Kullanıcıyı soft delete olarak işaretler.
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
