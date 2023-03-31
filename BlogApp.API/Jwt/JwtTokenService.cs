using BlogApp.API.Dtos;
using BlogApp.Messaging.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace BlogApp.API.Jwt
{
  public class JwtTokenService : IJwtTokenService
  {
    private const double EXPIRE_HOURS = 1.0;

    // 1 saatlik access token üreten servis
    public TokenDTO CreateToken(ClaimsIdentity identity)
    {
      var key = Encoding.ASCII.GetBytes(JWTSettings.SecretKey);
      var tokenHandler = new JwtSecurityTokenHandler();
      var descriptor = new SecurityTokenDescriptor
      {
        Subject = identity, // Token Sub kime ait olduğu
        Expires = DateTime.UtcNow.AddHours(EXPIRE_HOURS), // Ne kadar zaman geçerli olduğu
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Şifreleme algoritması
      };
      var token = tokenHandler.CreateToken(descriptor); // token oluştur
      var accessToken = tokenHandler.WriteToken(token); // token değeri access token olarak yaz.


      return new TokenDTO
      {
        AccessToken = accessToken,
        TokenType = "bearer"
      };
    }
  }
}
