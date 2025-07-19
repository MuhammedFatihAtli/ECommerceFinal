using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Commons;

namespace ECommerce.Domain.Entities
{
    public class Promotion:BaseEntity
    {
       
        public string Name { get; set; }
        public decimal DiscountRate { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
