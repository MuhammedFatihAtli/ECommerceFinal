using AutoMapper;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class ProductController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public ProductController(
            IServiceUnit service,
            UserManager<User> userManager,
            IProductService productService,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _service = service;
            _userManager = userManager;
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // Satıcının kendi ürünleri
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var seller = await _userManager.GetUserAsync(User);
            if (seller == null) return Unauthorized();

            var productDTOs = await _productService.GetProductsBySellerIdAsync(seller.Id);
            return View(productDTOs); // View expects IEnumerable<ProductDTO>
        }

        // Yeni ürün formu
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesAsync();
            return View(); // View: ProductCreateDTO
        }

        // Yeni ürün gönderimi
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync();
                return View(dto);
            }

            var seller = await _userManager.GetUserAsync(User);
            if (seller == null) return Unauthorized();

            dto.SellerId = seller.Id;

            await _productService.CreateProductAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var productDto = await _service.ProductService.GetProductByIdAsync(id);
            if (productDto == null) return NotFound();

            // ProductDTO → ProductEditDTO dönüşümü (AutoMapper veya manuel)
            var editDto = _mapper.Map<ProductEditDTO>(productDto);

            // Kategorileri getir, SelectList olarak ViewBag'e koy
            var categories = await _service.CategoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", editDto.CategoryId);

            return View(editDto);
        }

        // POST: Ürün düzenleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                // Model doğrulama hatalıysa, kategorileri tekrar yükle
                var categories = await _service.CategoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
                return View(dto);
            }

            try
            {
                await _service.ProductService.UpdateProductAsync(id, dto); // DTO tipi net olmalı
                TempData["Success"] = "Ürün başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Güncelleme sırasında hata oluştu: {ex.Message}");
                var categories = await _service.CategoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", dto.CategoryId);
                return View(dto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditStock(int id)
        {
            var dto = await _productService.GetProductByIdAsync(id);
            if (dto == null) return NotFound();

            return View(dto); // View: ProductDTO with only stock displayed
        }

        [HttpPost]
        public async Task<IActionResult> EditStock(int id, ProductEditDTO model)
        {
            var dto = await _productService.GetProductByIdAsync(id);
            if (dto == null) return NotFound();

            // Map ProductDTO to ProductEditDTO
            var editDto = new ProductEditDTO
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = model.Stock, // Only stock is updated
                CategoryId = dto.CategoryId                
            };

            await _productService.UpdateProductAsync(id, editDto);
            return RedirectToAction(nameof(Index));
        }

        // Yardımcı metot: Kategori listesini ViewBag'e doldur
        private async Task PopulateCategoriesAsync(int? selectedCategoryId = null)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedCategoryId);
        }
    }
}
