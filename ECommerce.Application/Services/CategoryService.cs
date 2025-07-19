using AutoMapper;
using ECommerce.Application.DTOs.CategoryDTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using ECommerce.Domain.Commons;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(CategoryCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BusinessRuleValidationException("Kategori adı boş olamaz!");

            bool exists = await _unitOfWork.CategoryRepository.AnyAsync(c => c.Name.ToLower().Equals(dto.Name.ToLower()));

            if (exists)
                throw new BusinessRuleValidationException("Bu isimde bir kategori zaten var!");

            var category = new Category(dto.Name, dto.Description ?? string.Empty);
            _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id, true);
            if (category == null)
                throw new NotFoundException("Kategori bulunamadı!");

            category.SoftDelete();
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync(bool isTrack = true, bool ignoreFilters = false)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(
                filter: null,
                isTrack: isTrack,
                ignoreFilters: ignoreFilters);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }




        public async Task<CategoryDTO> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id, true);
            if (category == null)
                throw new NotFoundException("Kategori bulunamad!");
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task UpdateAsync(CategoryEditDTO dto)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(dto.Id, true);
            if (category == null)
                throw new NotFoundException("Kategori bulunamadı!");

            // Aynı isimde başka kategori var mı kontrol et
            bool exists = await _unitOfWork.CategoryRepository.AnyAsync(c => 
                c.Name.ToLower().Equals(dto.Name.ToLower()) && c.Id != dto.Id);

            if (exists)
                throw new BusinessRuleValidationException("Bu isimde bir kategori zaten var!");

            category.Rename(dto.Name);
            category.UpdateDescription(dto.Description);

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
