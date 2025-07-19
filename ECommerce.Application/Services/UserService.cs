using AutoMapper;
using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using ECommerce.Domain.Commons;


namespace ECommerce.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task AddAsync(UserCreateDTO dto)
        {
            bool exists = await _unitOfWork.UserRepository.AnyAsync(u => u.UserName.ToLower() == dto.UserName.ToLower());

            if (exists)
                throw new BusinessRuleValidationException("Bu kullanıcı adı zaten mevcut!");

            var user = new User
            {
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                Status = true,
                CreatedDate = DateTime.UtcNow
            };
            
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id, true);

            if (user == null)
                throw new NotFoundException("Kullanıcı bulunamadı!");

            user.SoftDelete();
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync(bool isTrack = true)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync(
                filter: null,
                isTrack: isTrack);

            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                userDtos.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Status = user.Status
                });
            }

            return userDtos;
        }


        public async Task<UserDTO?> GetByIdAsync(int id, bool ignoreFilters = false)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return null;
            
            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Status = user.Status
            };
        }

        public async Task UpdateAsync(UserUpdateDTO dto)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(dto.Id, true);

            if (user == null)
                throw new NotFoundException($"Güncellenecek kullanıcı bulunamadı! Id: {dto.Id}");

            user.UserName = dto.UserName;
            user.FullName = dto.FullName;
            user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            }

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }
           
        public async Task<IEnumerable<UserDTO>> GetAllDeletedAsync()
        {
            var users = await _unitOfWork.UserRepository.FindConditionAsync(c => c.IsDeleted, ignoreFilters: true);

            if (users == null || !users.Any())
                throw new NotFoundException("Silinmiş kullanıcı bulunamadı!");

            var userDtos = new List<UserDTO>();
            foreach (var user in users)
            {
                userDtos.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Status = user.Status
                });
            }

            return userDtos;
        }              
        public async Task SoftDeleteAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id, true);

            if (user == null)
                throw new NotFoundException($"Silinen kullanıcı bulunamadı! Id: {id}");

            user.SoftDelete();
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id, true);

            if (user == null)
                throw new NotFoundException($"Silinen kullanıcı bulunamadı! Id: {id}");

            user.Restore();
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        

       
    }
}

