using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    // ProductRepository sınıfı, Product entity'si için veritabanı işlemlerini gerçekleştirir.
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Uncommented methods for clarity and functionality  
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public Task<List<Product>> GetProductsByGuestIdAsync(int? guestId)
        {
            return _context.Products
                .Where(p => p.GuestId == guestId)
                .ToListAsync();
        }

        public  async Task<List<Product>> GetProductsBySellerIdAsync(int sellerId)
        {
            return await _context.Products
                         .Where(p => p.SellerId == sellerId)
                         .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsWithCategoryAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id, bool includeCategory = false)
        {
            if (includeCategory)
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
