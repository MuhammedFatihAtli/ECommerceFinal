using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.UserDTOs
{
    // UserCreateDTO.cs, yeni bir kullanıcı oluşturmak için gerekli bilgileri tutar.
    public class UserCreateDTO
    {
        
        [Required(ErrorMessage = "Ad alanı boş geçilemez!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı alanı boş geçilemez!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Parola alanı boş geçilemez!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        public string Email { get; set; }

       
    }
}
