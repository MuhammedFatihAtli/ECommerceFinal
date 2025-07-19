using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface ISellerRepository : IGenericRepository<Seller>
    {
        Task AddAsync(Seller seller);
    }
}
