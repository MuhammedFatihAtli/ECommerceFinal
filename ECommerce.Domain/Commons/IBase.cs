using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Commons
{
    public interface IBase
    {
        int Id { get; set; }
        DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        bool IsDeleted { get; set; }
        void SoftDelete();
        void Restore();
    }
}
