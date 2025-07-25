using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    // PromotionRepository sınıfı, Promotion entity'si için veritabanı işlemlerini gerçekleştirir.
    public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
    {
        private readonly AppDbContext _context;
        public PromotionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
