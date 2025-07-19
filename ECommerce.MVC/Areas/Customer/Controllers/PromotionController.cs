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
        private readonly IServiceUnit _service;
        private readonly IPromotionService _promotionService;
        private readonly IProductService _productService;
        private readonly UserManager<ECommerce.Domain.Entities.User> _userManager;
        private readonly IMapper _mapper;

        public PromotionController(IPromotionService promotionService, IProductService productService, UserManager<User> userManager, IServiceUnit service, IMapper mapper)
        {
            _promotionService = promotionService;
            _productService = productService;
            _userManager = userManager;
            _service = service;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var seller = await _userManager.GetUserAsync(User);
            var products = await _productService.GetProductsBySellerIdAsync(seller.Id); // muhtemelen List<Product>

            var productDtos = _mapper.Map<List<ProductDTO>>(products); // Product -> ProductDTO dönüşümü

            var promotions = await _promotionService.GetAllAsync();

            var model = new PromotionVM
            {
                Products = productDtos, // Fix: Use the original 'products' list instead of 'productDtos'
                Promotions = promotions.ToList()
            };

            return View(model);
        }


    }
}

