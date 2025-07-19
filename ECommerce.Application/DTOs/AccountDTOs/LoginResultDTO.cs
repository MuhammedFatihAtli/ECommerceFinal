using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.AccountDTOs
{
    public class LoginResultDTO
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; } // Eğer JWT kullanıyorsan
        public int SellerId { get; set; } // Giriş yapan satıcıyı ayırt etmek için
        public bool IsSuccessful { get; internal set; }
        public string Message { get; internal set; }
        public string FullName { get; internal set; }
    }
}
