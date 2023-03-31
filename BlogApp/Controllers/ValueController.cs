using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using BlogApp.Messaging.Models;

namespace BlogApp.Controllers
{
  public class ValueController : Controller
  {

    private readonly HttpClient apiClient;


    // IHttpClientFactory ile ilgili clientların instancelarını altık.
    public ValueController(IHttpClientFactory httpClientFactory)
    {
      apiClient = httpClientFactory.CreateClient("apiClient");
    }


    public async Task<IActionResult> Index()
    {

      //var result1 = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);


      var accessToken = await HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, "access_token");
      // login olurken access_token store ettiğimiz için uygulama içerisinde store ettiğimiz yerden access_token istek yapıp kullanacağız.
      // authorization Header kısmına Bearer olarak Access Token set et
      apiClient.DefaultRequestHeaders.Authorization
                      = new AuthenticationHeaderValue("Bearer", accessToken);

      var result = await apiClient.GetAsync("api/values");

      if (result.IsSuccessStatusCode)
      {
        var response = await result.Content.ReadAsStringAsync();

        var data = System.Text.Json.JsonSerializer.Deserialize<List<GetValuesDto>>(response);

        return View(data);
      }
     
     
      return BadRequest();
      
    }
  }
}
