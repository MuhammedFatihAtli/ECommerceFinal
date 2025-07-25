using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.CustomerDTOs
{
    // CustomerCreateDTO.cs", yeni bir müşteri oluşturmak için gerekli bilgileri tutar.
    public class CustomerCreateDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }             
        public string UserName { get; set; }         
        public string Password { get; set; }
        public string Address { get; set; }
        public string? ProfileImageUrl { get; set; }
    }



}
