namespace ECommerce.Application.DTOs.SellerDTOs
{
    public class SellerUpdateDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
    }
}
