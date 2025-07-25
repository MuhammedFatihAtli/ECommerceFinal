using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.DTOs.CustomerDTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;

namespace ECommerce.Application.Services
{
    // CustomerService, müşteri yönetimi ile ilgili işlemleri yöneten bir hizmet sınıfıdır.
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // GetAllAsync metodu, tüm müşteri kayıtlarını asenkron olarak getirir.
        public async Task<List<CustomerListDTO>> GetAllAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllAsync(isTrack: false);
            return _mapper.Map<List<CustomerListDTO>>(customers);
        }
        // GetByIdAsync metodu, verilen müşteri kimliğine sahip müşteri kaydını asenkron olarak getirir.
        public async Task<CustomerDetailDTO> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id, false);
            if (customer == null)
                throw new NotFoundException("Müşteri bulunamadı!");

            return _mapper.Map<CustomerDetailDTO>(customer);
        }
        // AddAsync metodu, yeni bir müşteri kaydı ekler.
        public async Task AddAsync(CustomerCreateDTO dto)
        {
            // Sadece kullanıcı adı kontrolü
            var exists = (await _unitOfWork.CustomerRepository.GetAllAsync()).Any(c => c.UserName == dto.UserName);
            if (exists)
                throw new BusinessRuleValidationException("Bu kullanıcı adı zaten kayıtlı!");

            var customer = _mapper.Map<Customer>(dto);
            _unitOfWork.CustomerRepository.Add(customer);
            await _unitOfWork.SaveChangesAsync();
        }
        // UpdateAsync metodu, mevcut bir müşteri kaydını günceller.
        public async Task UpdateAsync(CustomerUpdateDTO dto)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(dto.Id);
            if (customer == null)
                throw new NotFoundException("Müşteri bulunamadı!");

            _mapper.Map(dto, customer);

            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();
        }
        // DeleteAsync metodu, verilen müşteri kimliğine sahip müşteri kaydını siler (soft delete).
        public async Task DeleteAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
            if (customer == null)
                throw new NotFoundException("Müşteri bulunamadı!");

            customer.SoftDelete();
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();
        }
        // LoginAsync metodu, müşteri giriş işlemini gerçekleştirir.
        public Task<LoginResultDTO> LoginAsync(LoginDTO dto)
        {
            // Sadece demo amaçlı false döndür
            return Task.FromResult(new LoginResultDTO { IsSuccess = false, Message = "Giriş işlemi desteklenmiyor." });
        }
        // RegisterAsync metodu, yeni bir müşteri kaydı oluşturur.
        public async Task<CustomerDetailDTO> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Kullanıcı adı boş olamaz!", nameof(userName));

            var customer = await _unitOfWork.CustomerRepository.FirstOrDefaultAsync(c => c.UserName == userName);

            if (customer == null)
                throw new NotFoundException("Müşteri bulunamadı!");

            return _mapper.Map<CustomerDetailDTO>(customer);
        }
    }
}
