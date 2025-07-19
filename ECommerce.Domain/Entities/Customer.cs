using ECommerce.Domain.Commons;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    public class Customer : User, IBase
    {

        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        // Navigation Properties
        public ICollection<Order> Orders { get; set; }

    }
}

