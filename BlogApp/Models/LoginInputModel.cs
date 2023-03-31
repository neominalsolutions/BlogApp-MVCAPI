using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
  public class LoginInputModel
  {

    [EmailAddress(ErrorMessage ="E-Posta formatında giriniz")]
    public string Email { get; set; }

    [Required(ErrorMessage ="Parola Boş geçilemez")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }


  }
}
