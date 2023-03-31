using BlogApp.Application.Services;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Services
{
  public class PostDomainService : IPostDomainService
  {
    /// <summary>
    /// Domain Service Infra katmanındaki Repositoryler üzerinden zayıf bağlı bir şekilde DbConnection açtı, ve DbEntityler üzerinde ssorgulama yaptı.
    /// </summary>
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IPostRepository _postRepository;


    public PostDomainService(ICategoryRepository categoryRepository, ITagRepository tagRepository, IPostRepository postRepository)
    {
      _categoryRepository = categoryRepository;
      _tagRepository = tagRepository;
      _postRepository = postRepository;
    }


    public void AddCommentToPost(Comment comment, Post model)
    {
      throw new NotImplementedException();
    }

    public void AddTagToPost(List<Tag> tags, Post model)
    {
      throw new NotImplementedException();
    }

    public void CreatePost(Post model)
    {
      // category Name var sa Id sini alıp set et
      // category yoksa db ye bak yeni bir category db ekle onun idsini referance ver.

      try
      {
        var dbCategory = _categoryRepository.FindAsync(x => x.Name == model.Category.Name).GetAwaiter().GetResult();

        if (dbCategory != null)
        {
          // post modele ilgili categoryName üzerinden doğru categoryId set ettik
          model.SetCategory(dbCategory.Id);
        }
        else
        {
          // kategori yoksa yeni kategori ekleyip dbye yansıttık
          var category = new Category(model.Category.Name);
           _categoryRepository.AddAsync(category).GetAwaiter().GetResult();
          model.SetCategory(category.Id);
        }

         _postRepository.AddAsync(model).GetAwaiter().GetResult();

      }
      catch (Exception)
      {
        throw new Exception("Gönderi girişi sırasında hata meydana geldi");
      }



    }

    public void RemoveCommentFromPost(Comment comment, Post model)
    {
      throw new NotImplementedException();
    }

    public bool UpdatePost(Post model)
    {
      throw new NotImplementedException();
    }

  
    void IPostDomainService.UpdatePost(Post model)
    {
      throw new NotImplementedException();
    }
  }
}
