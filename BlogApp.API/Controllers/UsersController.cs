using BlogApp.API.Dtos;
using BlogApp.Persistance.EF.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }


    [HttpPost]
    [Produces("application/json")] // json döndür
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(CreateUserDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] CreateUserDto model)
    {
      var user = new ApplicationUser();
      user.Email = model.Email;
      user.UserName = model.Email;

      // password Hash uygular
      var result = await _userManager.CreateAsync(user, model.Password);
      var claims = new List<Claim>();
      claims.Add(new Claim("LinkedInProfile", "https://www.linkedin/profiele/1000"));
      claims.Add(new Claim("DefaultLang", "en"));
    

      if (result.Succeeded)
      {
        var res2 = await _userManager.AddClaimsAsync(user, claims);
        var res3 = await _userManager.AddToRoleAsync(user, "Admin");

        return Created("", model);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "user oluşturuken bir hata meydana geldi");
      }
    }

  }
}
