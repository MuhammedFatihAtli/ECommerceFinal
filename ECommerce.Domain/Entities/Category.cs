using ECommerce.Domain.Commons;
using System;
using System.Collections.Generic;

namespace ECommerce.Domain.Entities
{
    /// <summary>
    /// Ürünlerin ait olduğu kategori bilgisini temsil eder.
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// Kategorinin adı.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Kategorinin açıklaması (isteğe bağlı).
        /// </summary>
        public string? Description { get; private set; }

        /// <summary>
        /// Parametre olarak sadece kategori adı alarak yeni bir kategori oluşturur.
        /// </summary>
        /// <param name="name">Kategori adı.</param>
        public Category(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Parametre olarak kategori adı ve açıklama alarak yeni bir kategori oluşturur.
        /// </summary>
        /// <param name="name">Kategori adı.</param>
        /// <param name="description">Kategori açıklaması.</param>
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Bu kategoriye ait ürünlerin listesidir.
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }

        /// <summary>
        /// Kategorinin adını günceller.
        /// </summary>
        /// <param name="newName">Yeni kategori adı.</param>
        public void Rename(string newName)
        {
            Name = newName;
            UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Kategorinin açıklamasını günceller.
        /// </summary>
        /// <param name="description">Yeni açıklama metni.</param>
        public void UpdateDescription(string description)
        {
            Description = description;
            UpdatedDate = DateTime.Now;
        }
    }
}

