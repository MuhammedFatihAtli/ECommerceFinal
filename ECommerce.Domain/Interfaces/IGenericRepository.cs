using ECommerce.Domain.Commons;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce.Domain.Interfaces
{
    /// <summary>
    /// Tüm entitylerde genel CRUD ve sorgulama işlemlerini tanımlayan generic repository arayüzü.
    /// </summary>
    /// <typeparam name="T">IBase arayüzünü implement eden entity tipi.</typeparam>
    public interface IGenericRepository<T> where T : IBase
    {
        /// <summary>
        /// Belirtilen id'ye sahip entity'yi getirir.
        /// </summary>
        /// <param name="id">Entity'nin ID'si.</param>
        /// <param name="ignoreFilters">Global query filtrelerini yok sayar mı?</param>
        /// <returns>Entity ya da null döner.</returns>
        Task<T?> GetByIdAsync(int id, bool ignoreFilters = false);

        /// <summary>
        /// Tüm entityleri filtre, include, izleme seçenekleriyle getirir.
        /// </summary>
        /// <param name="filter">Filtre lambda ifadesi (opsiyonel).</param>
        /// <param name="isTrack">Entitylerin izlenip izlenmeyeceği (default true).</param>
        /// <param name="ignoreFilters">Global filtrelerin yok sayılması (default false).</param>
        /// <param name="include">Include ifadeleri (örn: ilişkili veriler için).</param>
        /// <returns>Entity listesi.</returns>
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            bool isTrack = true,
            bool ignoreFilters = false,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        /// <summary>
        /// Belirtilen koşulu sağlayan entityleri getirir.
        /// </summary>
        /// <param name="condition">Filtre koşulu.</param>
        /// <param name="isTrack">Entity izleme durumu.</param>
        /// <param name="ignoreFilters">Global filtreleri yok sayma.</param>
        /// <returns>Entity listesi.</returns>
        Task<IEnumerable<T>> FindConditionAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false);

        /// <summary>
        /// Belirtilen koşulu sağlayan ilk veya default entity'yi getirir.
        /// </summary>
        /// <param name="condition">Filtre koşulu.</param>
        /// <param name="isTrack">Entity izleme durumu.</param>
        /// <param name="ignoreFilters">Global filtreleri yok sayma.</param>
        /// <returns>Entity ya da null.</returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> condition, bool isTrack = true, bool ignoreFilters = false);

        /// <summary>
        /// Filtreleme, sıralama, include ve seçim projeksiyonları ile liste getirir.
        /// </summary>
        /// <typeparam name="TResult">Dönüş tipi (DTO vb.).</typeparam>
        /// <param name="select">Projeksiyon ifadesi.</param>
        /// <param name="where">Filtre koşulu (opsiyonel).</param>
        /// <param name="orderBy">Sıralama fonksiyonu (opsiyonel).</param>
        /// <param name="join">Include fonksiyonu (opsiyonel).</param>
        /// <returns>Filtrelenmiş ve projekte edilmiş liste.</returns>
        Task<IEnumerable<TResult>> GetFilteredListAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null);

        /// <summary>
        /// Filtre, sıralama ve include ile tek bir kayıt getirir (projeksiyonlu).
        /// </summary>
        /// <typeparam name="TResult">Dönüş tipi.</typeparam>
        /// <param name="select">Projeksiyon ifadesi.</param>
        /// <param name="where">Filtre koşulu (opsiyonel).</param>
        /// <param name="orderBy">Sıralama fonksiyonu (opsiyonel).</param>
        /// <param name="join">Include fonksiyonu (opsiyonel).</param>
        /// <returns>Tekil sonuç ya da null.</returns>
        Task<TResult> GetFilteredFirstOrDefaultAsync<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> where = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> join = null);

        /// <summary>
        /// Belirtilen koşulu sağlayan kayıt var mı kontrol eder.
        /// </summary>
        /// <param name="condition">Koşul ifadesi.</param>
        /// <returns>Bool sonucu.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> condition);

        /// <summary>
        /// Belirtilen koşula göre kayıt sayısını döner.
        /// </summary>
        /// <param name="condition">Koşul ifadesi (opsiyonel).</param>
        /// <returns>Kayıt sayısı.</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> condition = null);

        /// <summary>
        /// Yeni entity'i ekler (senkron).
        /// </summary>
        /// <param name="item">Eklenecek entity.</param>
        void Add(T item);

        /// <summary>
        /// Var olan entity'i günceller.
        /// </summary>
        /// <param name="item">Güncellenecek entity.</param>
        void Update(T item);

        /// <summary>
        /// Entity'i kalıcı olarak siler.
        /// </summary>
        /// <param name="item">Silinecek entity.</param>
        void Delete(T item);

        /// <summary>
        /// Entity'i soft delete (işaretleyerek) siler.
        /// </summary>
        /// <param name="item">Soft delete yapılacak entity.</param>
        void SoftDelete(T item);

        /// <summary>
        /// Yeni entity'i asenkron olarak ekler.
        /// </summary>
        /// <param name="entity">Eklenecek entity.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Belirtilen koşula göre entity listesini getirir.
        /// </summary>
        /// <param name="predicate">Filtre ifadesi.</param>
        /// <returns>Entity listesi.</returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    }
}

