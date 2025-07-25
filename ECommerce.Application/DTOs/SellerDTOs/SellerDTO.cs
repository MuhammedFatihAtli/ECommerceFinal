namespace ECommerce.Application.DTOs.SellerDTOs
{
    // SellerDTO.cs", satıcı bilgilerini tutar.
    public class SellerDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
    }
}
