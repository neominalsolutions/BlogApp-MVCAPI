using BlogApp.API.Dtos;
using BlogApp.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace BlogApp.API.Jwt
{
    public interface IJwtTokenService
    {
    // ClaimsIdentity kimlik doğrulayan kullanıcıya ait taşınan özellikler. (UserName,Email,Role) InMemory olarask saklanır. Bu saklanan bilgiler ClaimsIdentity sınıfından gelir.
    TokenDTO CreateToken(ClaimsIdentity identity);
    }
}
