using ECommerce.Domain.Commons;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Domain.Entities
{
    public class User : IdentityUser<int>, IBase
    {
        public User(string fullName, string userEmail): base(userEmail)
        {
            FullName = fullName;
            Email = userEmail;
            UserName = userEmail;
        }
        public User()
        {
            
        }       
        public string? FullName { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Status { get; set; } = true;
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public void Restore()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                UpdatedDate = DateTime.Now;
            }
        }

        public void SoftDelete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                UpdatedDate = DateTime.Now;
            }
        }

       
    }
}
