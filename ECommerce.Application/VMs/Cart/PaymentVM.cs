using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.VMs.Cart
{
    public class PaymentVM
    {
        [Required]
        [Display(Name = "Kart Numarası")]
        [CreditCard]
        public string CardNumber { get; set; } //4111 1111 1111 1111


        [Required]
        [Display(Name = "Son Kullanma Tarihi")]
        public string ExpiryDate { get; set; } // MM/YY

        [Required]
        [Display(Name = "CVV")]
        [StringLength(4, MinimumLength = 3)]
        public string CVV { get; set; }

        [Required]
        [Display(Name = "Teslimat Adresi")]
        public string Address { get; set; }
    }
}
