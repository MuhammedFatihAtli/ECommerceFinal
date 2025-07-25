using AutoMapper;
using ECommerce.Application.DTOs.AccountDTOs;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ECommerce.Application.Services
{
    // AuthService.cs, kullanıcı kimlik doğrulama işlemlerini yönetir.
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;


        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Verilen kimlik bilgilerini kullanarak bir kullanıcıyı oturum açtırmayı dener.
        /// </summary>
        /// <remarks>
        /// Bu metot, sağlanan e-posta adresini kullanarak kullanıcıyı bulur ve şifresiyle kimlik doğrulaması yapmayı dener.
        /// Eğer kullanıcı bulunamazsa veya kimlik bilgileri geçersizse, açıklayıcı bir hata mesajı ile başarısızlık sonucu döner.
        /// </remarks>
        /// <param name="dto">Kullanıcının e-posta adresi ve şifresini içeren bir nesne.</param>
        /// <returns>
        /// Giriş denemesinin sonucunu belirten bir <see cref="AuthResult"/> nesnesi döner.
        /// Giriş başarılıysa <see cref="AuthResult.Success"/> döner; başarısızsa uygun hata mesajıyla birlikte
        /// <see cref="AuthResult.Failure(string)"/> sonucu döner.
        /// </returns>

        public async Task<AuthResult> LoginAsync(LoginDTO dto)
        {
            // Email ile User bul
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return AuthResult.Failure("Kullanıcı bulunamadı!");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.Password, false, false);
            if (result.Succeeded)
                return AuthResult.Success();

            return AuthResult.Failure("Invalid login attempt.");
        }

        /// <summary>
        /// Mevcut kullanıcıyı uygulamadan asenkron olarak oturumdan çıkarır.
        /// </summary>
        /// <remarks>
        /// Bu metot, kullanıcının kimlik doğrulama oturumunu temizler ve oturum durumunu geçersiz kılar.
        /// Kullanıcı uygulamadan açıkça çıkış yaptığında çağrılmalıdır.
        /// </remarks>
        /// <returns>
        /// Asenkron işlemi temsil eden bir görev (task) döner.
        /// </returns>
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Verilen kayıt bilgilerini kullanarak yeni bir kullanıcıyı asenkron olarak kaydeder.
        /// </summary>
        /// <remarks>
        /// Bu metot yeni bir kullanıcı hesabı oluşturur, kullanıcının durumunu aktif olarak ayarlar ve hesap oluşturma
        /// tarihini mevcut UTC zamanıyla belirler. Kayıt başarılı olursa, kullanıcı otomatik olarak oturum açmış olur.
        /// Kayıt başarısız olursa, metot ayrıntılı hata mesajlarıyla birlikte bir başarısızlık sonucu döner.
        /// </remarks>
        /// <param name="dto">Kullanıcının e-posta ve şifre gibi kayıt bilgilerini içeren bir nesne.</param>
        /// <returns>
        /// Kayıt işleminin sonucunu belirten bir <see cref="AuthResult"/> nesnesi döner.
        /// Kayıt başarılıysa <see cref="AuthResult.Success"/>; başarısızsa hata mesajlarıyla birlikte
        /// <see cref="AuthResult.Failure"/> sonucu döner.
        /// </returns>
        public async Task<AuthResult> RegisterAsync(RegisterDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            user.UserName = dto.Email;
            user.Status = true;
            user.CreatedDate = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return AuthResult.Success();
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return AuthResult.Failure(errors);
        }

        /// <summary>
        /// Şu anda kimliği doğrulanmış olan kullanıcının ID'sini döner.
        /// </summary>
        /// <param name="userPrincipal">
        /// Mevcut kullanıcıyı temsil eden <see cref="ClaimsPrincipal"/> nesnesi.
        /// Bu parametre <c>null</c> olamaz.
        /// </param>
        /// <returns>
        /// Kimliği doğrulanmış kullanıcının ID'si döner; eğer kullanıcı kimlik doğrulaması yapmamışsa <c>null</c> döner.
        /// </returns>
        public async Task<int?> GetCurrentUserIdAsync(ClaimsPrincipal userPrincipal)
        {
            var user = await _userManager.GetUserAsync(userPrincipal);
            return user?.Id;
        }

        /// <summary>
        /// Belirtilen <see cref="ClaimsPrincipal"/> ile ilişkili mevcut kullanıcıyı döner.
        /// </summary>
        /// <param name="userPrincipal">
        /// Mevcut kullanıcı bağlamını temsil eden <see cref="ClaimsPrincipal"/> nesnesi.
        /// </param>
        /// <returns>
        /// Asenkron işlemi temsil eden bir görev (task) döner.
        /// Görev sonucu, mevcut kullanıcıyı temsil eden <see cref="User"/> nesnesini içerir; 
        /// eğer kullanıcı bulunamazsa <c>null</c> döner.
        /// </returns>

        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            return await _userManager.GetUserAsync(userPrincipal);
        }

        /// <summary>
        /// Belirtilen kullanıcıyla ilişkili rol listesini döner.
        /// </summary>
        /// <param name="user">
        /// Rolleri alınacak kullanıcı. Null olamaz.
        /// </param>
        /// <returns>
        /// Asenkron işlemi temsil eden bir görev (task) döner.
        /// Görev sonucu, belirtilen kullanıcıya ait rol adlarının bir listesini içerir.
        /// Eğer kullanıcının hiç rolü yoksa liste boş olacaktır.
        /// </returns>
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }

}
