using BlogApp.API.Dtos;
using BlogApp.API.SwaggerFilters;
using BlogApp.Messaging.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlogApp.API.Controllers
{
  // Api controllerlar mvc conteollerlardan farklı olarak bir resource verdiklerinden dolayı best practise s takılı yazılır. 

  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // Access Token göndermeden bu controller'a bağlanamayacağız.
  public class ValuesController : ControllerBase
  {

    public List<GetValuesDto> GetValues
    {
      get
      {

        var dtos = new List<GetValuesDto>();
        dtos.Add(new GetValuesDto { Id = 1, Name = "Value-1" });
        dtos.Add(new GetValuesDto { Id = 2, Name = "Value-2" });
        dtos.Add(new GetValuesDto { Id = 3, Name = "Value-3" });

        return dtos;
      }
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
   

      return Ok(GetValues);
    }



    [Produces("application/json")] // json döndür
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(GetValuesDto))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    [HttpGet("getBy/{id:int?}")] // int ama opsiyone bir parametredir.
    [SwaggerOperationFilter(typeof(OptionalRouteParameterOperationFilter))]
    public async Task<IActionResult> Get(int? id = null)
    {
   
      if(id == null)
      {
        return NotFound();
      }

      try
      {
        var dto = GetValues.FirstOrDefault(x => x.Id == id);
        if (dto == null)
          return NotFound();
        else
          return Ok(await Task.FromResult(dto));
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Sunucuda beklenmedik bir hata meydana geldi");
      }

    }

    // Querystring yöntemi

    [HttpGet("detail")]
    public async Task<IActionResult> GetDetail(int? id = null)
    {

      if (id == null)
      {
        return NotFound();
      }

      try
      {
        var dto = GetValues.FirstOrDefault(x => x.Id == id);
        if (dto == null)
          return NotFound();
        else
          return Ok(await Task.FromResult(dto));
      }
      catch (Exception)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, "Sunucuda beklenmedik bir hata meydana geldi");
      }

    }


    [HttpPost]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(PostValuesDto))]
    public async Task<IActionResult> Post([FromBody] PostValuesDto dto)
    {
      // 201 result
      // bir şeyi post edeceksek create result döndürmek best practise
      return Created($"/api/values/detail?id={dto.Id}", dto);
    }


    [HttpPut("{id:int}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(NoContentResult))]
    public async Task<IActionResult> Put(int id, [FromBody] PutValuesDto dto)
    {
      try
      {
        var p = GetValues.FirstOrDefault(x => x.Id == id);
        p.Name = dto.Name;

        // 204 result
        // bir şeyi post edeceksek create result döndürmek best practise
        return NoContent();
      }
      catch (Exception ex)
      {

        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }

     
    }

    // Delete başarılı ise NoContent döndrür.

    [HttpDelete("{id:int}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent, type: typeof(NoContentResult))]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        var p = GetValues.FirstOrDefault(x => x.Id == id);
        GetValues.Remove(p);

        // 204 result
        // bir şeyi post edeceksek create result döndürmek best practise
        return NoContent();
      }
      catch (Exception ex)
      {

        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
      }


    }

  }
}
