using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.VMs.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email alanı boş geçilemez!")]
        [EmailAddress(ErrorMessage = "Email adresi formatına uygun bir giriş yapınız!")]
        [Display(Name = "E-Mail: ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez!")]
        [Display(Name = "Şifre: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla: ")]
        public bool RememberMe { get; set; }
    }
}
