using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Interfaces;
using ECommerce.Domain.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Services
{
    // ProductService, ürün yönetimi ile ilgili işlemleri yöneten bir hizmet sınıfıdır.
    public class ProductService : IProductService
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // CreateProductAsync metodu, yeni bir ürün oluşturur.
        public async Task CreateProductAsync(ProductCreateDTO productCreateDto)
        {
            bool exists = await _unitOfWork.ProductRepository.AnyAsync(p => p.Name.ToLower().Equals(productCreateDto.Name.ToLower()));
            if (exists)
            {
                throw new Exception("Bu ürün adı zaten mevcut");
            }
            string imagePath = null;
            if (productCreateDto.ImageFile != null && productCreateDto.ImageFile.Length > 0)
            {
                // Dosya boyutu kontrolü (5MB)
                if (productCreateDto.ImageFile.Length > 5 * 1024 * 1024)
                {
                    throw new Exception("Dosya boyutu 5MB'dan büyük olamaz");
                }

                // Dosya türü kontrolü
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(productCreateDto.ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new Exception("Sadece JPG, PNG ve GIF formatları desteklenir");
                }

                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                // Klasör yoksa oluştur
                var directory = Path.GetDirectoryName(uploadPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await productCreateDto.ImageFile.CopyToAsync(stream);
                }

                imagePath = "/images/" + fileName;
            }
            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                Stock = productCreateDto.Stock,
                CategoryId = productCreateDto.CategoryId,
                ImagePath = imagePath ?? string.Empty,
                SellerId = productCreateDto.SellerId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false
            };


            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveChangesAsync();


        }
        // UpdateProductAsync metodu, mevcut bir ürünü günceller.
        public async Task UpdateProductAsync(int id, ProductEditDTO productDto)
        {
            var existingProduct = await _unitOfWork.ProductRepository.GetByIdAsync(id, true);
            if (existingProduct == null)
                throw new Exception("Ürün bulunamadı");

            bool isChanged = false;

            if (productDto.ImageFile != null && productDto.ImageFile.Length > 0)
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingProduct.ImagePath?.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productDto.ImageFile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await productDto.ImageFile.CopyToAsync(stream);
                }

                existingProduct.ImagePath = "/images/" + fileName;
                isChanged = true;
            }

            if (existingProduct.Name != productDto.Name)
            {
                existingProduct.Name = productDto.Name;
                isChanged = true;
            }

            if (existingProduct.Description != productDto.Description)
            {
                existingProduct.Description = productDto.Description;
                isChanged = true;
            }

            if (existingProduct.Price != productDto.Price)
            {
                existingProduct.Price = productDto.Price;
                isChanged = true;
            }

            if (existingProduct.Stock != productDto.Stock)
            {
                existingProduct.Stock = productDto.Stock;
                isChanged = true;
            }

            if (existingProduct.CategoryId != productDto.CategoryId)
            {
                existingProduct.CategoryId = productDto.CategoryId;
                isChanged = true;
            }
            if (existingProduct.PromotionId != productDto.PromotionId)
            {
                existingProduct.PromotionId = productDto.PromotionId;
                isChanged = true;
            }
            if (isChanged)
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }




        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, true);
            if (product == null)
            {
                throw new Exception("Ürün bulunamadı");
            }
            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(bool isTrack = true, bool ignoreFilters = false)
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync(
                filter: null,
                include: p => p.Include(x => x.Category),
                isTrack: isTrack,
                ignoreFilters: ignoreFilters);

            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }


        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id, true);
            if (product == null)
            {
                throw new Exception("Ürün bulunamadı");
            }
            if (product.Category == null)
                throw new Exception("Ürünün kategorisi bulunamadı");
            return _mapper.Map<ProductDTO>(product);
        }

        
       

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryIdAsync(int? categoryId)
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync(
        include: p => p.Include(x => x.Category)
                       .Include(x => x.Seller) //  SELLER BİLGİLERİ DE ÇEKİLİYOR
    );

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            // Opsiyonel: Sadece silinmemiş ve aktif ürünler gelsin
            products = products.Where(p => !p.IsDeleted);

            return _mapper.Map<IEnumerable<ProductDTO>>(products);

        }
       
        public async Task<IEnumerable<ProductDTO>> GetProductsBySellerIdAsync(int sellerId)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsBySellerIdAsync(sellerId);
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsForGuestAsync(string sessionId)
        {
            // Şimdilik sessionId kullanılmıyor çünkü misafire özel ürün ayrımı yapılmıyor
            var products = await _unitOfWork.ProductRepository.GetAllAsync(
                include: p => p.Include(x => x.Category).Include(x => x.Seller),
                filter: p => !p.IsDeleted // sadece aktif ürünler
            );

            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

    }
}
