using ECommerce.Domain.Commons;
using System;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Kullanıcının sisteme veya belirli bir hizmete kayıt bilgisini temsil eder.
    /// </summary>
    public class Enrollment : BaseEntity
    {
        /// <summary>
        /// Sadece userId ile yeni bir kayıt nesnesi oluşturur.
        /// </summary>
        /// <param name="userId">Kayıt olan kullanıcının ID'si.</param>
        public Enrollment(int userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// EF Core gibi araçlar için boş kurucu metod.
        /// </summary>
        public Enrollment()
        {
        }

        /// <summary>
        /// Kayıt yapan kullanıcının ID'si.
        /// Sadece oluşturma sırasında atanabilir.
        /// </summary>
        public int UserId { get; private set; }

        /// <summary>
        /// Kayıt yapan kullanıcıya ait kullanıcı nesnesi.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Kayıt tarihi. Varsayılan olarak oluşturulma zamanıdır.
        /// </summary>
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
