using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs.UserDTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Durum")]
        public bool Status { get; set; }

        [Display(Name = "Rol")]
        public string? Role { get; set; }

       
    }
}
