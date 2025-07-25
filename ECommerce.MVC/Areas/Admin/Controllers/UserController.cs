using AutoMapper;
using ECommerce.Application.DTOs.UserDTOs;
using ECommerce.Application.UnitOfWorks;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.MVC.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // Servis ve yöneticilerin tanımlanması
        private readonly IServiceUnit _service;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;



        // Constructor ile bağımlılıkların enjekte edilmesi
        public UserController(IServiceUnit service, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _service = service;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Kullanıcıları listele
        public async Task<IActionResult> Index()
        {
            try
            {
                // Tüm kullanıcıları veritabanından al
                var users = await _userManager.Users.ToListAsync();

                var userList = new List<UserDTO>();

                // Her kullanıcı için rol bilgisiyle birlikte DTO listesi oluştur
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    userList.Add(new UserDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Email = user.Email,
                        Status = user.Status,
                        Role = roles.FirstOrDefault() ?? "Rol Yok",
                        
                    });
                }
                // Listeyi view'e gönder
                return View(userList);
            }
            catch (Exception ex)
            {// Hata varsa boş liste ve hata mesajı gönderilir
                TempData["Error"] = ex.Message;
                return View(new List<UserDTO>());
            }
        }

        
        // Yeni kullanıcı formunu döner
        [HttpGet]
        public IActionResult Create()
        {
            // Roller ViewBag ile view'e gönderilir
            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList(); // Roller ViewBag ile View'e taşınıyor
            return View();
        }

        // Yeni kullanıcı kaydı
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDTO dto, string[] selectedRoles)
        {
            // Model valid değilse sayfa geri döner
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
                Status = true,// Varsayılan olarak aktif



                CreatedDate = DateTime.UtcNow
            };
            // Kullanıcıyı veritabanına ekle (şifre hashlenir)
            // Identity ile kullanıcıyı ekle (şifre hashlenir)
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                // Kullanıcı başarıyla oluşturulduysa roller atanır
                // Seçilen rolleri ata
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
            // Hatalar ModelState'e eklenir
            // Hataları modele ekle
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            // Roller yeniden yüklenir ve form geri döner
            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
            return View(dto);
        }

        // Kullanıcıyı düzenleme formu
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            // DTO nesnesi doldurulur
            var dto = new UserUpdateDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email
            };
            // Roller ve mevcut kullanıcı rolleri ViewBag ile gönderilir
            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);

            return View(dto);
        }

        // Kullanıcıyı güncelleme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateDTO dto, string[] selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (user == null) return NotFound();

            if (!ModelState.IsValid)
            {
                // Validasyon hataları varsa geri döner
                ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
                ViewBag.UserRoles = await _userManager.GetRolesAsync(user);
                return View(dto);
            }

            // Bilgileri güncelle
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

            // Şifre değiştirilmişse güncelle
            if (!string.IsNullOrEmpty(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                
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

        // Kullanıcıyı silme
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
        // Toplam kullanıcı sayısını JSON olarak döner (dashboard için kullanılabilir)
        [HttpGet]
        public async Task<IActionResult> GetUserCount()
        {
            var count = await _userManager.Users.CountAsync();
            return Json(count);
        }
    }
}
