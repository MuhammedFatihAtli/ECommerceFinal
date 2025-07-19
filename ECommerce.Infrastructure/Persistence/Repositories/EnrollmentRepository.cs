using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Enrollment>> GetAllWithUserAsync()
    {
        return await _context.Enrollments
                             .Include(e => e.User)
                             .ToListAsync();
    }

    public async Task<List<Enrollment>> GetByUserIdWithUserAsync(int userId)
    {
        return await _context.Enrollments
                             .Include(e => e.User)
                             .Where(e => e.UserId == userId)
                             .ToListAsync();
    }
}

