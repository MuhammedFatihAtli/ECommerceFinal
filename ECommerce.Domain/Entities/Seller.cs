using ECommerce.Domain.Commons;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Seller : User, IBase
    {
        public int SellerId { get; set; }
        public string CompanyName { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }

        // Navigation Properties
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }               
        public void Restore()
        {
            throw new NotImplementedException();
        }

        public void SoftDelete()
        {
            throw new NotImplementedException();
        }

        //Proje sonunda bakılacak.!!!!!!!!!
        //public string IdentityUserId { get; set; } // AspNetUsers tablosuna foreign key
        //public IdentityUser IdentityUser { get; set; }

    }
}
