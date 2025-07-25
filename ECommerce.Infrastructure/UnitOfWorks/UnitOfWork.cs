using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.UnitOfWorks
{
    //repository'leri tek bir yerden erişilebilir sağlama .(Tek noktadan tüm veritabanı işlemleri yönetme)
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(
    AppDbContext context,
    ICategoryRepository categoryRepository,
    IProductRepository productRepository,
    IEnrollmentRepository enrollmentRepository,
    IUserRepository userRepository,
    IOrderRepository orderRepository,
    ISellerRepository sellerRepository,
    IPromotionRepository promotionRepository,
    ICustomerRepository customerRepository,
    ICommentRepository commentRepository//**
)
        {
            _context = context;
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
            EnrollmentRepository = enrollmentRepository;
            UserRepository = userRepository;
            OrderRepository = orderRepository;
            SellerRepository = sellerRepository;
            PromotionRepository = promotionRepository;
            CustomerRepository = customerRepository;
            CommentRepository = commentRepository;// **
        }

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }      
        public IEnrollmentRepository EnrollmentRepository { get; }
        public IUserRepository UserRepository { get; }
        public IOrderRepository OrderRepository { get; }

        public ISellerRepository SellerRepository { get; }
        public IPromotionRepository PromotionRepository { get; }

        public ICustomerRepository CustomerRepository { get; }
        public ICommentRepository CommentRepository { get; }

        public int SaveChanges()
        {
            var result = _context.SaveChanges();
            if (result > 0)
                return result;
            else
                throw new Exception("Değişiklik işlenemedi!");
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return result;
            else
                throw new Exception("Değişiklik işlenemedi!");
        }

        //DB işlemlerini bir işlem bloğu olarak başlar
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
                _transaction = await _context.Database.BeginTransactionAsync();
        }

        //Her şey doğruysa değişiklikleri onayla
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        //Hata olursa geri al (iptal et).
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        
    }
}
