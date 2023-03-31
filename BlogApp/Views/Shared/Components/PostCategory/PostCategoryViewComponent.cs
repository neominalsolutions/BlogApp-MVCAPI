using BlogApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Views.Shared.Components.PostCategory
{
  // modellerin arayüz için controllerdan soyutlaşmış bir şekilde çekildiği kısım.
  // ViewComponent Kalıtım alıp IViewComponent Result tipinde dönüş tiğini tanımlarız.
  public class PostCategoryViewComponent:ViewComponent
  {
    private readonly ICategoryRepository _categoryRepository;

    public PostCategoryViewComponent(ICategoryRepository categoryRepository)
    {
      _categoryRepository = categoryRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      var clist = (await _categoryRepository.ListAsync());

      

      // view model döndürülmesini tavsiye ediyoruz.

      return View(clist.OrderBy(x=> x.Name).ToList());
    }
  }
}
