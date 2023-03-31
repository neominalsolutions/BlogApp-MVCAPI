using BlogApp.API.Jwt;
using BlogApp.Persistance.EF.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BlogApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _tokenService;

    public AuthController(IJwtTokenService tokenService, UserManager<ApplicationUser>  userManager)
    {
      _tokenService = tokenService;
      _userManager = userManager;
    }

    // attributed based routing feature
    // api/auth/token access token generate edeceğiz
    [HttpPost("token")]
    public async Task<IActionResult> CreateToken([FromBody] LoginDto loginDto)
    {

      var user =  await _userManager.FindByEmailAsync(loginDto.Email);
      var passCheck = await _userManager.CheckPasswordAsync(user, loginDto.Password);



      if (user != null && passCheck)
      {

        var userRoles = await _userManager.GetRolesAsync(user);

        var identity = new ClaimsIdentity(new Claim[]
        {
      // sistem access token içerisinde tutulacak olan payload bilgisi
                    new Claim("Name", user.Email),
                    new Claim("Role", JsonSerializer.Serialize(userRoles) ),
                    new Claim("UserId",user.Id)
        });

        var token = _tokenService.CreateToken(identity);

        return Ok(token);

      }

      return BadRequest();

    }

  }
}
