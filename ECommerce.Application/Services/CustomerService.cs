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
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CustomerListDTO>> GetAllAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllAsync(isTrack: false);
            return _mapper.Map<List<CustomerListDTO>>(customers);
        }

        public async Task<CustomerDetailDTO> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id, false);
            if (customer == null)
                throw new NotFoundException("Müşteri bulunamadı!");

            return _mapper.Map<CustomerDetailDTO>(customer);
        }

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

        public async Task UpdateAsync(CustomerUpdateDTO dto)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(dto.Id);
            if (customer == null)
                throw new NotFoundException("Müşteri bulunamadı!");

            _mapper.Map(dto, customer);

            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
            if (customer == null)
                throw new NotFoundException("Müşteri bulunamadı!");

            customer.SoftDelete();
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task<LoginResultDTO> LoginAsync(LoginDTO dto)
        {
            // Sadece demo amaçlı false döndür
            return Task.FromResult(new LoginResultDTO { IsSuccess = false, Message = "Giriş işlemi desteklenmiyor." });
        }

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
