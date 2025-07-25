using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Application.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PromotionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // CreateAsync metodu, yeni bir promosyon oluşturur ve veritabanına kaydeder.
        public async Task CreateAsync(Promotion promotion)
        {
             _unitOfWork.PromotionRepository.Add(promotion);
            await _unitOfWork.SaveChangesAsync();
        }
        // DeleteAsync metodu, verilen promosyon kimliğine sahip promosyonu siler.
        public async Task DeleteAsync(int id)
        {
            var promotion = await _unitOfWork.PromotionRepository.GetByIdAsync(id);
            if (promotion == null)
                throw new Exception("Silinecek promosyon bulunamadı.");

            _unitOfWork.PromotionRepository.Delete(promotion);
            await _unitOfWork.SaveChangesAsync();
        }

        // GetAllAsync metodu, tüm promosyonları asenkron olarak getirir.
        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            return await _unitOfWork.PromotionRepository.GetAllAsync();
        }
        // GetByIdAsync metodu, verilen promosyon kimliğine sahip promosyonu asenkron olarak getirir.
        public async  Task<Promotion> GetByIdAsync(int id)
        {
            return await _unitOfWork.PromotionRepository.GetByIdAsync(id);
        }

        // UpdateAsync metodu, mevcut bir promosyonu günceller.
        public async Task UpdateAsync(Promotion promotion)
        {
            _unitOfWork.PromotionRepository.Update(promotion);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
