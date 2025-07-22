using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {      
        Task<List<Comment>> GetCommentsByProductIdAsync(int productId);
    }
}
