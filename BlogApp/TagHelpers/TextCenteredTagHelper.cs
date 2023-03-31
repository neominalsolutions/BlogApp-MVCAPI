using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVCLab2.TagHelpers
{
  // Tag Helpers, razor sayfalardaki server-side kodları HTML elementleri olarak oluşturmayı sağlayan; View'ın daha okunabilir, anlaşılabilir ve kolay geliştirilebilir hale gelmesine olanak tanıyan, . NET Core ile birlikte gelip, Html Helper kullanımlarının yerini almış yapılardır.


  // attribute tanımı yaparken kebab-case formatta yazalım. Html önerilen format
  // HtmlTargetElement ile elementi html de hangi isimle çağıracağımızı tanımlarız.
  [HtmlTargetElement("text-centered", Attributes = "content-text")]

    
    public class TextCenteredTagHelper : TagHelper
    {
      // Html InnerText ContentText ismi ile tanımladık. Html elementine tanımlanmış attribute ismi CamelCase formatta yazalım
      public string ContentText { get; set; }


      // HTML çıktı üreteceğiz
      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
        // TagHelperOutput ile dış kapsayıcısı üzerinden değişiklik yaparız

        output.TagName = "p"; // p elementi ile dış kapsayıcısını saralım
        output.Attributes.Add("style", "text-align:center");
        output.Content.SetContent(ContentText);
        base.Process(context, output);
      }

    }
  }
