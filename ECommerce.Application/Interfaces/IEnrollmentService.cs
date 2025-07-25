using ECommerce.Application.DTOs.EnrollmentDTOs;

namespace ECommerce.Application.Interfaces
{
    // IEnrollmentService.cs", kullanıcı kayıt işlemleri için gerekli servis arayüzünü tanımlar.
    public interface IEnrollmentService
    {
        Task<List<ProfileDTO>> GetProfilesAsync();
        Task<List<ProfileDTO>> GetProfileWithUserIdAsync(int userId);
    }
}
