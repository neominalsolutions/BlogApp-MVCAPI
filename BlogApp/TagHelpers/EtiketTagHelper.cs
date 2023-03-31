using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlogApp.TagHelpers
{

  //<post-tag text=""></post-tag>


  [HtmlTargetElement("etiket")]
  public class EtiketTagHelper : TagHelper
  {

    [HtmlAttributeName("text")]
    public string Text { get; set; }

    [HtmlAttributeName("onClick")]
    public string OnClickEventName { get; set; }

    [HtmlAttributeName("actionUrl")]
    public string ActionUrl { get; set; }




    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
      // arayüze tag helper'ı yansıtmamız sağlayan bir kısım
      // burada arayüz ile ilgili işlemler yapıyoruz.

      // output arayüze yansıtılacak olan çıktı
      // context de tag helper'ın instance

      // açma kapama tagi olsun
      output.TagMode = TagMode.StartTagAndEndTag;

      // elementin innerText değerini değiştirik

      output.Content.SetHtmlContent($"<a href='{ ActionUrl}' onClick='{OnClickEventName}' class='btn btn-primary'>#{Text}</a>");

      //output.Content.AppendHtml();

      base.Process(context, output);

    }
  }
}
