namespace ECommerce.Application.DTOs.CustomerDTOs
{
    public class CustomerUpdateDTO
    {
        public int Id { get; set; }                   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Password { get; set; }        
    }



}
