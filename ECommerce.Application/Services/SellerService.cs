using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.DTOs.SellerDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Application.Services
{
    public class SellerService : ISellerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SellerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // SellerService, satıcı yönetimi ile ilgili işlemleri yöneten bir hizmet sınıfıdır.
        public async Task AddAsync(SellerCreateDTO dto)
        {
            var seller = _mapper.Map<Seller>(dto);
            await _unitOfWork.SellerRepository.AddAsync(seller);
            await _unitOfWork.SaveChangesAsync();
        }
        // DeleteAsync metodu, verilen satıcı kimliğine sahip satıcıyı siler.
        public async Task DeleteAsync(int id)
        {
            var seller = await _unitOfWork.SellerRepository.GetByIdAsync(id);
            if (seller == null)
                throw new Exception("Seller not found");

            _unitOfWork.SellerRepository.SoftDelete(seller); // IsDeleted flag'ini ayarlayan method
            await _unitOfWork.SaveChangesAsync();
        }

        // GetAllAsync metodu, tüm satıcıları asenkron olarak getirir.
        public async Task<List<SellerDTO>> GetAllAsync()
        {
            var sellers = await _unitOfWork.SellerRepository.GetAllAsync(isTrack: false);
            return _mapper.Map<List<SellerDTO>>(sellers);
        }
        // GetByIdAsync metodu, verilen satıcı kimliğine sahip satıcıyı asenkron olarak getirir.
        public async Task<SellerDTO> GetByIdAsync(int id)
        {
            var seller = await _unitOfWork.SellerRepository.GetByIdAsync(id);
            if (seller == null)
                throw new Exception("Seller not found");

            return _mapper.Map<SellerDTO>(seller);
        }

        // GetByUserNameAsync metodu, verilen kullanıcı adına sahip satıcıyı asenkron olarak getirir.
        public async Task<SellerDTO> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new Exception("UserName is null or empty!");

            // Önce UserName ile, bulamazsa Email ile ara
            var seller = await _unitOfWork.SellerRepository.FirstOrDefaultAsync(s => s.UserName == userName);
            if (seller == null)
            {
                seller = await _unitOfWork.SellerRepository.FirstOrDefaultAsync(s => s.Email == userName);
            }
            if (seller == null)
                throw new Exception($"Seller not found for userName/email: {userName}");

            if (_mapper == null)
                throw new Exception("Mapper is null!");

            try
            {
                return _mapper.Map<SellerDTO>(seller);
            }
            catch (Exception ex)
            {
                throw new Exception($"Mapping error: {ex.Message}", ex);
            }
        }
        // LoginAsync metodu, satıcı giriş işlemini asenkron olarak gerçekleştirir.
        public async Task<LoginResultDTO> LoginAsync(LoginDTO dto)
        {
            var seller = await _unitOfWork.SellerRepository
         .FirstOrDefaultAsync(s => s.Email == dto.Email && s.Password == dto.Password && !s.IsDeleted, isTrack: false);

            if (seller == null)
                return new LoginResultDTO { IsSuccessful = false, Message = "Invalid email or password" };

            return new LoginResultDTO
            {
                IsSuccessful = true,
                Message = "Login successful",
                SellerId = seller.Id,
                FullName = $"{seller.FirstName} {seller.LastName}"
            };
        }

        // UpdateAsync metodu, mevcut bir satıcıyı günceller.
        public async Task UpdateAsync(SellerUpdateDTO dto)
        {
            var seller = await _unitOfWork.SellerRepository.GetByIdAsync(dto.Id);
            if (seller == null)
                throw new Exception("Seller not found");

            _mapper.Map(dto, seller);
            _unitOfWork.SellerRepository.Update(seller);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
