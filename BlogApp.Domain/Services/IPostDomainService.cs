using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Services
{
  // Modeller üzerinden Bussiness Caselere göre sürecin bussiness'ını Domain Service ile yönetiyoruz.
    public interface IPostDomainService
    {
      void CreatePost(Post model);
      void AddCommentToPost(Comment comment, Post model);

    /// <summary>
    /// Yorum Kaldırma
    /// </summary>
    /// <param name="comment"></param>
    /// <param name="model"></param>
      void RemoveCommentFromPost(Comment comment, Post model);

    /// <summary>
    /// İlgili gönderi için Post Modele tagleri gir
    /// </summary>
    /// <param name="tags"></param>
    /// <param name="model"></param>
    /// <returns></returns>
      void AddTagToPost(List<Tag> tags, Post model);

      void UpdatePost(Post model);

    }
}
