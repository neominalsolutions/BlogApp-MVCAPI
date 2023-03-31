using BlogApp.Domain.Entities;
using BlogApp.Domain.Repositories;
using BlogApp.Infra.Persistance.EF.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Views.Shared.Components.PostList
{
  public class PostListViewComponent:ViewComponent
  {
    private readonly IPostRepository _postRepository;

    public PostListViewComponent(IPostRepository postRepository)
    {
      _postRepository = postRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? categoryName, int? tagId, string? searchText, string sortBy = "asc", int? limit = null, int? currentPage = null)
    {
      var model = new List<Post>();

      tagId = (tagId == 0 ? null : tagId);

      searchText = searchText == "null" ? null : searchText;


      // asQuarable tipini repoya eklememiz lazım


      if (string.IsNullOrEmpty(categoryName) && tagId == null)
      {
        model = await _postRepository.ListAsync();

      }
      else if(string.IsNullOrEmpty(categoryName) && tagId!=null)
      {
        model = await _postRepository.WhereAsync(x => x.Tags.Any(x=> x.Id == tagId));

      } else if(!string.IsNullOrEmpty(categoryName) && tagId == null)
      {
        model = await _postRepository.WhereAsync(x => EF.Functions.Like(x.Category.Name,categoryName));
     
      }
      else if (!string.IsNullOrEmpty(categoryName) && tagId != null)
      {
        model = await _postRepository.WhereAsync(x => EF.Functions.Like(x.Category.Name, categoryName) && x.Tags.Any(x => x.Id == tagId));
     
      }

      if(limit != null && currentPage != null)
      {
        // liste üzerinde sayfalama yaptık
        // sayfalı bir şekilde veri çekme yöntemi
        model.Skip((int)limit * (int)currentPage).Take((int)limit).ToList();
      }

      if(sortBy == "asc")
      {
        model = model.OrderBy(x => x.Title).ToList();
      } else
      {
        model = model.OrderByDescending(x => x.Title).ToList();
      }

      if(!string.IsNullOrEmpty(searchText)) // dbde filtreleme yapmaz
      {
        // Ramde EF.Functions.Like kullanamayız. veri tabanı ile ilgili bir kullanım
        model = model.Where(x => x.Title.Trim().ToLower().Contains(searchText.Trim().ToLower())).ToList();
      }



      return View(model);

    }
  }
}
