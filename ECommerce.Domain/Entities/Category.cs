using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Category : BaseEntity
    {
        
        public string Name { get; private set; }
        public string? Description { get; private set; }

        public Category(string name)
        {
            Name = name;
        }

        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual ICollection<Product> Products { get; set; }

        public void Rename(string newName) 
        { 
            Name = newName;
            UpdatedDate = DateTime.Now;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
            UpdatedDate = DateTime.Now;
        }
    }
}
