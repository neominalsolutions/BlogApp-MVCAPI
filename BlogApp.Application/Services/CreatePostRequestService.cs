using BlogApp.Application.Models;
using BlogApp.Domain.Entities;
using BlogApp.Persistance.EF.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Services
{

  /// <summary>
  /// Son kullanıcıdan gelen iş isteklerinin koordine edildiği yer.
  /// </summary>
  public class CreatePostRequestService : ICreatePostRequestService
  {

    // Application Serviceden InfraServicler (LoggerService, MailService,Repository, DomainService)
    private readonly IPostDomainService _postDomainService;

    public CreatePostRequestService(IPostDomainService postDomainService)
    {
      _postDomainService = postDomainService;
    }

    public async Task<CreatePostResponse> HandleAsync(CreatePostRequest request)
    {
      // bütün bu post create ile ilgili altyapısal veri merkezli bussiness case durumlarını yönetebiliriz. exception durumları

      // program nesneleri olan Dto gibi request nesnelerini, Entitylere dönüştürür
      // auto mapper kütüphanesi kullanılabilir
      var post = new Post(title: request.Title, content: request.Content);
      //post.Title = "sadsad";
      post.SetTitle("wdsadsa");
      //post.Commments.add

      //var p = new Post(title: "title-1", content: "content-1");
      //p.AddComment(commentBy: "ali", CommentText: "Merhaba");
      //p.AddTag("#yazilim");
      //p.AddTag("#clean_architecture");

      post.Category = new Category(request.CategoryName);
      post.SetPostedBy("Mert");


      foreach (var tagItem in request.Tags)
      {
        post.AddTag(tagItem);
      }

      // yeni bir gönderinin sisteme girilmesini sağlar
      // Domain Katmanı
      _postDomainService.CreatePost(post);

      return await Task.FromResult(default(CreatePostResponse));
     
    }
  }
}
