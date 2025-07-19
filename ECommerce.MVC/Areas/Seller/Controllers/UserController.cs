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

        public UserController(IServiceUnit service, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _service = service;
            _userManager = userManager;
            _roleManager = roleManager;
        }

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
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList(); 
            return View();
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDTO dto, string[] selectedRoles)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
                return View(dto);
            }

            var user = new User
            {
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                Status = true,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
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

            ViewBag.Roles = _roleManager.Roles.Select(x => x.Name).ToList();
            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);

            return View(dto);
        }

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

            if (selectedRoles != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRolesAsync(user, selectedRoles);
            }

            TempData["Success"] = "Kullanıcı başarıyla güncellendi!";
            return RedirectToAction(nameof(Index));
        }

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

