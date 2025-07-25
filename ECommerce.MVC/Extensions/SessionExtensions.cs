using Newtonsoft.Json;

namespace ECommerce.MVC.Extensions
{


    // Session işlemleri için genişletme (extension) metotlarını içeren static sınıf
    public static class SessionExtensions
    {
        // ISession arayüzü için genişletme metodu: Bir nesneyi session’a kaydeder
        public static void SetObject(this ISession session, string key, object value)
        { // 'value' parametresi JSON formatına çevrilir ve 'key' anahtarı ile session'a kaydedilir
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        // ISession arayüzü için genişletme metodu: Session’dan nesne okur
        public static T GetObject<T>(this ISession session, string key)
        {// 'key' anahtarıyla session’dan string olarak veri alınır
            var value = session.GetString(key);

            // Eğer veri yoksa (null ise), T tipinin varsayılan değeri döndürülür
            // Varsa, JSON string’i T tipine dönüştürülerek döndürülür
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
