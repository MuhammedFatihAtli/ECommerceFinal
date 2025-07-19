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

namespace ECommerce.MVC.Controllers
{
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

            IEnumerable<ProductDTO> productDtos;

            if (seller != null)
            {
                var products = await _productService.GetProductsBySellerIdAsync(seller.Id);
                productDtos = _mapper.Map<IEnumerable<ProductDTO>>(products);
            }
            else
            {
                // Misafir için, örneğin boş liste veya guestId ile ürün çekme
                productDtos = new List<ProductDTO>();
                // Alternatif: productDtos = await _productService.GetProductsByGuestIdAsync(guestId);
            }

            var promotions = await _promotionService.GetAllAsync();

            var model = new PromotionVM
            {
                Products = productDtos.ToList(),
                Promotions = promotions.ToList()
            };

            return View(model);
        }



    }
}

