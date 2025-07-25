using global::ECommerce.Application.Interfaces;
using global::ECommerce.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ECommerce.Infrastructure.Security
{


    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        public string GenerateToken(User user, IList<string> roles)//kullanıcının bilgilerini ve rollerini alır,  buna göre bir JWT üretir.
        {

            // bilgiler sayesinde backend , gelen token'ı okuduğunda kimin işlem yaptığını anlar.
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? "")
        };

            //Kullanıcının sahip olduğu her rol için bir Role claim’i eklenir.(Bu sayede token’ı alan kişi "Admin", "Customer" yetkileriyle işlem yapabilir.)
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            //şifreleme anahtarı ve algoritması belirlenir. Bu bilgiler, JWT’nin güvenli bir şekilde imzalanmasını sağlar.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,// kim üretti
                audience: _jwtSettings.Audience,// kimler kullanabilir
                claims: claims,//Kullanıcı bilgileri ve roller.
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),//süresi ne kadar geçerli
                signingCredentials: creds);//hangi algoritma ve anahtarla imzalanacak.

            return new JwtSecurityTokenHandler().WriteToken(token);// JWT nesnesini string formatına çevirir,frontend’e veya HTTP Header’a gönderilir.
        }
    }

}
