using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.MVC.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class UserController : Controller
    {
        private readonly IServiceUnit _service;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        // Constructor: Gerekli servislerin bağımlılık enjeksiyonu
        public UserController(IServiceUnit service, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _service = service;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // Kullanıcı listesini getirir
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                var users = await _service.UserService.GetAllAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<UserDTO>());
            }
        }
        // Kullanıcı oluşturma formunu gösterir
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList(); 
            return View();
        }
        // Yeni kullanıcı oluşturur
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDTO dto, string[] selectedRoles)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
                return View(dto);
            }
            // Yeni kullanıcı nesnesi oluştur
            var user = new User
            {
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                Status = true,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);// Identity ile kullanıcı oluştur

            if (result.Succeeded)
            {
                // Rol atamaları
                if (selectedRoles != null)
                {
                    foreach (var role in selectedRoles)
                    {
                        if (await _roleManager.RoleExistsAsync(role))
                            await _userManager.AddToRoleAsync(user, role);
                    }
                }

                TempData["Success"] = "Kullanıcı başarıyla oluşturuldu!";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
            return View(dto);
        }
        // Kullanıcı düzenleme sayfası (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();

            var dto = new UserUpdateDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email
            };

            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();// Tüm roller
            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);// Mevcut roller

            return View(dto);
        }
        // Kullanıcıyı güncelleme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateDTO dto, string[] selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
                ViewBag.UserRoles = await _userManager.GetRolesAsync(user);
                return View(dto);
            }
            // Kullanıcı bilgilerini güncelle
            user.UserName = dto.UserName;
            user.FullName = dto.FullName;
            user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);
           
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
                ViewBag.UserRoles = await _userManager.GetRolesAsync(user);
                return View(dto);
            }
            // Şifre değişikliği varsa
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                // Şifre değiştirilecekse
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
                    ViewBag.UserRoles = await _userManager.GetRolesAsync(user);
                    return View(dto);
                }
            }
            // Rolleri güncelle
            if (selectedRoles != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRolesAsync(user, selectedRoles);
            }

            TempData["Success"] = "Kullanıcı başarıyla güncellendi!";
            return RedirectToAction(nameof(Index));
        }

        // Kullanıcı silme işlemi

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    TempData["Error"] = "Kullanıcı bulunamadı!";
                    return RedirectToAction(nameof(Index));
                }

                await _userManager.DeleteAsync(user);
                TempData["Success"] = "Kullanıcı başarıyla silindi!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

