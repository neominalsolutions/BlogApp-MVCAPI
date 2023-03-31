using BlogApp.API.Dtos;
using BlogApp.Messaging.Models;
using BlogApp.Persistance.EF.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RolesController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RolesController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
      _userManager = userManager;
      _roleManager = roleManager;
    }


    [HttpPost]
    [Produces("application/json")] // json döndür
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(CreateRoleDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post([FromBody] CreateRoleDto model)
    {
      var user = new ApplicationUser();
      user.Email = "mert@test.com";
      user.UserName = "mert.alptekin";

      // user role assign etme
      //await userManager.AddToRoleAsync(user,model.Name);

      var role = new ApplicationRole();
      role.Name = model.Name;
      role.Description = model.Description;

      var result = await _roleManager.CreateAsync(role);

      if(result.Succeeded)
      {
        return Created("", model);
      }
      else
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "role oluşturuken bir hata meydana geldi");
      }

      
    }
  }
}
