using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "Beklemede")]
        Pending = 0,

        [Display(Name = "Onaylandı")]
        Confirmed = 1,

        [Display(Name = "Kargoya Verildi")]
        InTransit = 2,

        [Display(Name = "Teslim Edildi")]
        Delivered = 3,

        [Display(Name = "İptal Edildi")]
        Cancelled = 4
    }
}
