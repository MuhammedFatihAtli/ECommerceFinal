using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Commons
{
    public abstract class BaseEntity : IBase
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;

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
