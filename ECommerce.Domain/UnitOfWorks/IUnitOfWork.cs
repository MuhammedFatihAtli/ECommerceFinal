using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.UnitOfWorks
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IEnrollmentRepository EnrollmentRepository { get; }
        IUserRepository UserRepository { get; }
        IOrderRepository OrderRepository { get; }
        ISellerRepository SellerRepository { get; }
        IPromotionRepository PromotionRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        ICommentRepository CommentRepository { get; }
        /// <summary>
        /// Değişiklikleri veritabanına kaydeder (async).
        /// </summary>
        /// <returns>Kaydedilen kayıt sayısı.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Değişiklikleri veritabanına kaydeder (senkron).
        /// </summary>
        /// <returns>Kaydedilen kayıt sayısı.</returns>
        int SaveChanges();

        /// <summary>
        /// Yeni bir veritabanı işlemi (transaction) başlatır.
        /// </summary>
        /// <returns>İşlem tamamlanana kadar bekler.</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Başlatılmış veritabanı işlemini (transaction) onaylar (commit).
        /// </summary>
        /// <returns>İşlem tamamlanana kadar bekler.</returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// Başlatılmış veritabanı işlemini (transaction) geri alır (rollback).
        /// </summary>
        /// <returns>İşlem tamamlanana kadar bekler.</returns>
        Task RollbackTransactionAsync();


    }
}
