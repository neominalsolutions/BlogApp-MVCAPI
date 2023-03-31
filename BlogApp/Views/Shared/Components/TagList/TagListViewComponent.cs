using BlogApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Views.Shared.Components.TagList
{
  public class TagListViewComponent:ViewComponent
  {
    private readonly ITagRepository tagRepository;

    public TagListViewComponent(ITagRepository tagRepository)
    {
      this.tagRepository = tagRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync(int? postId = null)
    {
      // postId makale bazlı tagleri getirebilmek için opsiyonel olarak kullanabiliriz.

      var tags = await tagRepository.ListAsync();

      return View(tags);
    }
  }
}
