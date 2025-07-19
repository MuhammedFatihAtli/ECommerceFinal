namespace ECommerce.Application.DTOs.CustomerDTOs
{
    public class CustomerDetailDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public string Address { get; set; }
        public string? ProfileImageUrl { get; set; }

        public DateTime CreatedDate { get; set; }
    }



}
