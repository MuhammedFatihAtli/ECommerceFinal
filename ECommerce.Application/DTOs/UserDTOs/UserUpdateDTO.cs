using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı boş geçilemez!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla {1} karakter olabilir")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Yeni Şifre")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az {2} karakter olmalıdır")]
        public string? Password { get; set; }

        // [Required(ErrorMessage = "Role alanı boş geçilemez!")]
        // public string Role { get; set; }
    }
}
