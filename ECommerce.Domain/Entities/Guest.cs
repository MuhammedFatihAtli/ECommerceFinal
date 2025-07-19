using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Guest:User,IBase
    {        
        public int GuestId { get; set; }
        
        public ICollection<Order> Orders { get; set; }
        public ICollection<Product> Products { get; set; }
        public void Restore()
        {
            throw new NotImplementedException();
        }

        public void SoftDelete()
        {
            throw new NotImplementedException();
        }
    }
}
