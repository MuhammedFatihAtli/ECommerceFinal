using AutoMapper;
using ECommerce.Application.DTOs.EnrollmentDTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<List<ProfileDTO>> GetProfilesAsync();
        Task<List<ProfileDTO>> GetProfileWithUserIdAsync(int userId);
    }
}
