using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public Enrollment(int userId)
        {
            UserId = userId;
         
        }
        public Enrollment()
        {
            
        }
        public int UserId { get; private set; }
        public virtual User User { get; set; }
      
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
