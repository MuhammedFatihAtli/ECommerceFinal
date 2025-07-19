using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<List<Enrollment>> GetAllWithUserAsync();
    Task<List<Enrollment>> GetByUserIdWithUserAsync(int userId);
}

