namespace ECommerce.Application.DTOs.SellerDTOs
{
    // SellerCreateDTO.cs", satıcı oluşturma işlemleri için gerekli bilgileri tutar.
    public class SellerCreateDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
