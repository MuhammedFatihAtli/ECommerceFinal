using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.VMs.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Kullanıcı adı boş geçilemez!")]
        [Display(Name = "Kullanıcı Adı: ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [EmailAddress(ErrorMessage = "Email adresi formatına uygun bir giriş yapınız!")]
        [Display(Name = "E-Mail: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        [Display(Name = "Şifre: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar: ")]
        [Compare("Password", ErrorMessage = "Girilen şifreler tutarsız!")]
        public string ConfirmPassword { get; set; }
    }
}
