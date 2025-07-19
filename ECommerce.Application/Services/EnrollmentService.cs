using AutoMapper;
using ECommerce.Application.DTOs.EnrollmentDTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProfileDTO>> GetProfilesAsync()
        {
            var enrollments = await _unitOfWork.EnrollmentRepository.GetAllWithUserAsync();
            return _mapper.Map<List<ProfileDTO>>(enrollments);
        }

        public async Task<List<ProfileDTO>> GetProfileWithUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new BusinessRuleValidationException("Geçersiz kullanıcı ID");

            var enrollments = await _unitOfWork.EnrollmentRepository.GetByUserIdWithUserAsync(userId);

            if (enrollments == null || !enrollments.Any())
                throw new NotFoundException("Bu kullanıcıya ait kayıt bulunamadı!");

            return _mapper.Map<List<ProfileDTO>>(enrollments);
        }
    }
}
