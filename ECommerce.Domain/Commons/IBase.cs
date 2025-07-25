using System;

namespace ECommerce.Domain.Commons
{
    /// <summary>
    /// Tüm temel varlıklar için standart özellik ve davranışları tanımlar.
    /// </summary>
    public interface IBase
    {
        /// <summary>
        /// Varlığın benzersiz kimliği.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Varlığın oluşturulma tarihi.
        /// </summary>
        DateTime CreatedDate { get; set; }

        /// <summary>
        /// Varlığın en son güncellenme tarihi. Null olabilir.
        /// </summary>
        DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Varlığın silinmiş olup olmadığını belirten bayrak (soft delete için).
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Varlığı soft delete ile siler (işaretleyerek silme).
        /// </summary>
        void SoftDelete();

        /// <summary>
        /// Soft delete ile silinen bir varlığı geri yükler.
        /// </summary>
        void Restore();
    }
}

