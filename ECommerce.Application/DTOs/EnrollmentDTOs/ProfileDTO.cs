namespace ECommerce.Application.DTOs.EnrollmentDTOs
{
    // ProfileDTO.cs", kullanıcı profili bilgilerini tutar.
    public class ProfileDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public int UserId { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
