namespace ECommerce.Application.DTOs.CustomerDTOs
{
    public class CustomerListDTO
    {
        public int Id { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
    }



}
