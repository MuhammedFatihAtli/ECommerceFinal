using AutoMapper;
using ECommerce.Application.DTOs.ProductDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Application.VMs.Promotion;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.MVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer")]
    public class PromotionController : Controller
    {
        private readonly IServiceUnit _service;// Tüm servisleri bir arada barındıran servis birimi
        private readonly IPromotionService _promotionService;// Promosyon işlemleri için servis
        private readonly IProductService _productService; // Ürün işlemleri için servis
        private readonly UserManager<ECommerce.Domain.Entities.User> _userManager;// Kullanıcı bilgilerine erişim
        private readonly IMapper _mapper;// DTO ve entity dönüşümleri için AutoMapper


        // Constructor: DI (Dependency Injection) ile servislerin alınması
        public PromotionController(IPromotionService promotionService, IProductService productService, UserManager<User> userManager, IServiceUnit service, IMapper mapper)
        {
            _promotionService = promotionService;
            _productService = productService;
            _userManager = userManager;
            _service = service;
            _mapper = mapper;
        }
        // Kullanıcının ürünlerine ait promosyonları ve ürünleri görüntüler
        public async Task<IActionResult> Index()
        {
            // Oturumdaki kullanıcı bilgilerini al
            var seller = await _userManager.GetUserAsync(User);
            // Kullanıcının satıştaki ürünlerini al
            var products = await _productService.GetProductsBySellerIdAsync(seller.Id); // muhtemelen List<Product>
            // Entity nesnelerini DTO nesnelerine dönüştür
            var productDtos = _mapper.Map<List<ProductDTO>>(products); // Product -> ProductDTO dönüşümü
            // Tüm promosyonları al (Not: filtreleme yok, iyileştirilebilir)
            var promotions = await _promotionService.GetAllAsync();
            // ViewModel oluştur ve View'a gönder
            var model = new PromotionVM
            {
                Products = productDtos,// Kullanıcının ürünleri
                Promotions = promotions.ToList()// Sistemdeki tüm promosyonlar (iyileştirme: kullanıcıya özel filtre eklenebilir)
            };

            return View(model);
        }


    }
}

