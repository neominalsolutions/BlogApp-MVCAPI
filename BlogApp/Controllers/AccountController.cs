using BlogApp.Messaging.Models;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers
{
  public class AccountController : Controller
  {
    private readonly HttpClient apiClient;


    // IHttpClientFactory ile ilgili clientların instancelarını altık.
    public AccountController(IHttpClientFactory httpClientFactory)
    {
      apiClient = httpClientFactory.CreateClient("apiClient");
    }

    public IActionResult Login()
    {

      //using (HttpClient http = new HttpClient())
      //{
      //  http.
      //}

      return View();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {

      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      return RedirectToAction("Login", "Account");

    }


      [HttpPost]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
      // 1. apiya bağlanıp token al
      // 2. token parse et
      // 3. access token store et
      // 4. cookie based auth ile login ol


      var payload = System.Text.Json.JsonSerializer.Serialize(model);
      // jsonString
      StringContent content = new StringContent(payload, Encoding.UTF8, "application/json");
      // jsonFormatında bir string oldu
      // HttpResponseMessage apidan döncecek olan status code result bilgilerine erişebiliriz.
      HttpResponseMessage tokenResponse = await apiClient.PostAsync("api/auth/token", content);

      var token = await tokenResponse.Content.ReadFromJsonAsync<TokenDTO>();
      // token bilgisi içindeki payloadlarına inip almamız lazım


      var handler = new JwtSecurityTokenHandler();
      // token decode etmek için JwtSecurityTokenHandler bir sınıf kullandık

      // kontrol amaçlı token okunup okunamadığına baktık
      if (handler.CanReadToken(token.AccessToken))
      {

        // xyzx.dsada.43534
        // JWT ReadJwtToken method ile decode edildi.
        var jsonToken = handler.ReadJwtToken(token.AccessToken);

     

        // NameClaimType,RoleClaimType ezdik.

        var identity = new ClaimsIdentity(jsonToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "Name", "Role");

        // oturum açacak olan kişinin kimlik bilgilerini oluşturuyoruz.
        // kimlik (identity) üzerinde taşınacak olan değerler jsonToken.Claims kullanmıcıya atanan verilen değerler.
        // bu kimlik için bir cookie scheme oluştur, sisteme bu cookie üzerinden login olucaz, Kimlik doğrulayan kişinin Name ve Role bilgileri bu şekilde isimlerden maplensin.

        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        var exp = identity.Claims.First(x=> x.Type == "exp");


        // http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name

        // elimizdeki token değerini cookie üzerinde şifrelenmiş bir şekilde saklamak için authProps store tokens özelliğini kullandık
        // daha sonra istek atarken bu özellik üzerinden access_token değerimizi okuyacağız.
        var authProps = new AuthenticationProperties();
        authProps.IsPersistent = model.RememberMe; // cookie session bazlı yada expiration bazlı olmasını burası ayarlar.
        authProps.ExpiresUtc = DateTime.UtcNow.AddHours(1);
        var tokens = new List<AuthenticationToken>();
        tokens.Add(new AuthenticationToken { Name = "access_token", Value = token.AccessToken });
        authProps.StoreTokens(tokens);

        // şu scheme üzerinden , principal kimliği ile, şu authProps ile login ol oturum aç.
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);
        // Cookie Based Authentication yaptık.

        return Redirect("/");
      }


      return View();
    }
  }
}
