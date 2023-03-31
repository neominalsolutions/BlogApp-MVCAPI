using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
  public class LoggingController : Controller
  {
    public IActionResult Index()
    {
      throw new Exception("Hata");

      return View();
    }
  }
}
