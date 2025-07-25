using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Application.VMs.Promotion;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class PromotionController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly IPromotionService _promotionService;
        private readonly IProductService _productService;
        private readonly UserManager<ECommerce.Domain.Entities.User> _userManager;

        public PromotionController(IPromotionService promotionService, IProductService productService, UserManager<ECommerce.Domain.Entities.User> userManager, IServiceUnit service)

        {
            _promotionService = promotionService;
            _productService = productService;
            _userManager = userManager;
            _service = service;
        }
        // Promosyon sayfası: Ürünleri ve aktif promosyonları listeler
        public async Task<IActionResult> Index()
        {
            var seller = await _userManager.GetUserAsync(User);// Giriş yapan satıcı
            var products = await _productService.GetProductsBySellerIdAsync(seller.Id); // Satıcının ürünleri
            var promotions = await _promotionService.GetAllAsync();// Tüm promosyonlar

            return View(new PromotionVM
            {
                Products = products.ToList(),// ViewModel'e ürünler atanır
                Promotions = promotions.ToList()// ViewModel'e promosyonlar atanır
            });
        }

        // Yeni promosyon oluşturma formunu gösterir
        public async Task<IActionResult> Create()
        {
            var seller = await _userManager.GetUserAsync(User);
            var products = await _productService.GetProductsBySellerIdAsync(seller.Id);

            var model = new PromotionCreateVM
            {
                Products = products.ToList()// View'e gönderilecek ürün listesi
            };

            return View(model);
        }
        // Yeni promosyon oluşturma işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PromotionCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                // Model geçerli değilse ürünler yeniden yüklenip form tekrar gösterilir
                var seller = await _userManager.GetUserAsync(User);
                model.Products = (await _productService.GetProductsBySellerIdAsync(seller.Id)).ToList();
                return View(model);
            }


            // Yeni promosyon nesnesi oluşturulur

            var promotion = new Promotion
            {
                Name = model.Name,
                DiscountRate = model.DiscountRate,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
            };

            await _promotionService.CreateAsync(promotion);// Veritabanına kaydedilir
            // Seçilen ürüne promosyon atanır
          
            var productDto = await _productService.GetProductByIdAsync(model.SelectedProductId);
            if (productDto != null)
            {
                var editDto = new ProductEditDTO
                {
                    Id = productDto.Id,
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Stock = productDto.Stock,
                    CategoryId = productDto.CategoryId,
                    PromotionId = promotion.Id,// Yeni oluşturulan promosyon atanır
                    ImagePath = productDto.ImagePath,
                    ImageFile = null
                };

                await _productService.UpdateProductAsync(productDto.Id, editDto);// Ürün güncellenir
            }

            return RedirectToAction(nameof(Index));
        }


        // Ürüne promosyon ata (GET)
        public async Task<IActionResult> AssignPromotion(int productId)
        {
            var seller = await _userManager.GetUserAsync(User);
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null || product.SellerId != seller.Id)
                return Unauthorized(); // Satıcıya ait değilse yetkisiz erişim

            var promotions = await _promotionService.GetAllAsync();
            ViewBag.Promotions = new SelectList(promotions, "Id", "Name", product.PromotionId); // Seçilebilir promosyon listesi

            return View(product); // Ürün bilgisiyle form açılır
        }

        // Ürüne promosyon ata (POST)
        [HttpPost]
        public async Task<IActionResult> AssignPromotion(int productId, int promotionId)
        {
            var seller = await _userManager.GetUserAsync(User);
            var productDto = await _productService.GetProductByIdAsync(productId);

            if (productDto == null || productDto.SellerId != seller.Id)
                return Unauthorized();// Yetkisiz işlem
            // Ürün güncelleme DTO'su hazırlanır

            var editDto = new ProductEditDTO
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                CategoryId = productDto.CategoryId,
                PromotionId = promotionId, // Seçilen promosyon atanır
                ImagePath = productDto.ImagePath,
                ImageFile = null
            };

            await _productService.UpdateProductAsync(productId, editDto);// Ürün güncellenir

            return RedirectToAction("Index");
        }

        // Promosyon silme işlemi

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var promotion = await _promotionService.GetByIdAsync(id);
            if (promotion == null)
                return NotFound();

            await _promotionService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

