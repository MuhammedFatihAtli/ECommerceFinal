using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Belirtilen enum (sayımlama) değerinin görünen adını döndürür.
        /// </summary>
        /// <remarks>
        /// Bu metot, ilgili enum üyesinin <see cref="DisplayAttribute"/> özniteliğiyle tanımlanıp tanımlanmadığını
        /// yansıtma (reflection) kullanarak kontrol eder. Eğer böyle bir öznitelik bulunamazsa, varsayılan olarak 
        /// <see cref="object.ToString"/> metodunun sonucunu döndürür.
        /// </remarks>
        /// <param name="enumValue">Görünen adı alınmak istenen enum değeri.</param>
        /// <returns>
        /// Eğer enum üyesi <see cref="DisplayAttribute"/> ile tanımlanmışsa, <see cref="DisplayAttribute.Name"/> özelliğinin değeri; 
        /// aksi takdirde enum değerinin string (metin) temsili döndürülür.
        /// </returns>

        public static string GetDisplayName(this Enum enumValue)
        {
            var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            if (member == null) return enumValue.ToString();

            var displayAttr = member.GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name ?? enumValue.ToString();
        }
    }
}
