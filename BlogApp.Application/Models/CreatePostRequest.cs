using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Models
{
  // Api için Dto Modelimizi temsil eder
  // Kullanıcıdan alınacak olan Inputları burada tanımladık
  // Use-Case durumlarına göre gelen iş istekleri nesnesi
  public class CreatePostRequest
  {
    [Required(ErrorMessage = "Title alanı boş geçilemez")]
    [MaxLength(50,ErrorMessage = "Title alanı 50 karakterden fazla olamaz")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content alanı boş geçilemez")]
    [MaxLength(200, ErrorMessage = "Content alanı 200 karakterden fazla olamaz")]
    public string Content { get; set; }

    [Required(ErrorMessage = "Tags alanı boş geçilemez")]
    public List<string> Tags { get; set; } = new List<string>();

    // login olan kullanıcıdan yakalayabiliriz
    public string PostedBy { get; set; }

    [Required(ErrorMessage = "CategoryName alanı boş geçilemez")]
    public string CategoryName { get; set; }



  }
}
