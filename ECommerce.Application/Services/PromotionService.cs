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
        public  async Task CreateAsync(Promotion promotion)
        {
             _unitOfWork.PromotionRepository.Add(promotion);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var promotion = await _unitOfWork.PromotionRepository.GetByIdAsync(id);
            if (promotion == null)
                throw new Exception("Silinecek promosyon bulunamadı.");

            _unitOfWork.PromotionRepository.Delete(promotion);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            return await _unitOfWork.PromotionRepository.GetAllAsync();
        }

        public async  Task<Promotion> GetByIdAsync(int id)
        {
            return await _unitOfWork.PromotionRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Promotion promotion)
        {
            _unitOfWork.PromotionRepository.Update(promotion);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
