using AutoMapper;
using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Application.VMs.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.MVC.Controllers
{
   
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IMapper _mapper;

        public AccountController(IAuthService authService, IMapper mapper, IEnrollmentService enrollmentService)
        {
            _authService = authService; 
            _enrollmentService = enrollmentService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginDto = _mapper.Map<LoginDTO>(model);
            var result = await _authService.LoginAsync(loginDto);

            if (result.IsSuccess)
            {
                // Kullanıcıyı email ile bul
                var user = await _authService.GetCurrentUserAsync(User);
                if (user != null)
                {
                    var roles = await _authService.GetUserRolesAsync(user);
                    if (roles != null && roles.Count > 0)
                    {
                        var role = roles[0]; // Varsayılan olarak ilk rolü al
                        if (role == "Admin")
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }
                        else if (role == "Seller")
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Seller" });
                        }
                        else if (role == "Customer")
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Customer" });
                        }
                    }
                }
                // Rol bulunamazsa ana sayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registerDto = _mapper.Map<RegisterDTO>(model);
            var result = await _authService.RegisterAsync(registerDto);

            if (result.IsSuccess)
                return RedirectToAction("Index", "Home");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error);

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = await _authService.GetCurrentUserIdAsync(User);
            if (userId == null)
                return RedirectToAction("Login");

            // Fix: Explicitly cast 'userId' to 'int' since 'GetProfileWithUserId' expects an 'int' argument  
            var profile = await _enrollmentService.GetProfileWithUserIdAsync((int)userId);
            if (profile == null)
                return View("ProfileNotFound");

            return View(profile);
        }
    }

}
