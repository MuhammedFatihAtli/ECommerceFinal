using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.AccountDTOs
{
    public class LoginDTO
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
