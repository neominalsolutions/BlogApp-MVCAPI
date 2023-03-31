
using BlogApp.Domain.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Entities
{
  // EF daha önceki EF core öncesi versiyonlarda Lazy loading olarak çalışırdı.
  // EF Core da Eager Loading (Explicit Loading) Post ile birlikte benim tanımladığım nesneleri getir. Include()
  // POCO Object
  public class Post : BaseEntity<string>, IDeletedEntity
  {
    public string Title { get; private set; }
    public string Content { get; private set; }

    public string PostedBy { get; private set; } // User Id

    // EF 6.0 ile gelen bu özellik ile string nullable olan değerler ? ile tanımanmaz ise db işlemlerinde hata veriyor.
    public string? DeletedBy { get; set; } // nullable
    public DateTime? DeletedAt { get; set; }

    // FK alanımız
    public string CategoryId { get; set; }

    // post nesnesi üzerinden postun category değerine erişmek için koyduk
    public Category Category { get; set; }



    // navigation property
    // field üzerinden commentleri yöneteceğim fakat dışarıdan add methodlu çağırılmasın diye de IReadOnlyCollection yaptık
    private List<Comment> _comments = new List<Comment>();
    public IReadOnlyCollection<Comment> Commments => _comments;

    // Tag yönetimi

    // 1 post birden fazla tag içerebilir.
    private List<Tag> _tags = new List<Tag>();
    public IReadOnlyCollection<Tag> Tags => _tags;


    public Post(string title, string content)
    {
      SetTitle(title);
      SetContent(content);
      this.Id = Guid.NewGuid().ToString();
    }

    public void SetTitle(string title)
    {
      if (string.IsNullOrEmpty(title))
      {
        throw new Exception("Title değeri boş geçildi");
      }

      Title = title.Trim();
    }

    public void SetContent(string content)
    {
      Content = content;
    }

    public void SetPostedBy(string postedBy)
    {
      PostedBy = postedBy;
    }


    public void Delete(string deletedBy)
    {
      DeletedBy = deletedBy;
      DeletedAt = DateTime.Now;
    }

    public void AddComment(string commentBy, string CommentText)
    {
      _comments.Add(new Comment(commentBy, CommentText));

    }

    /// <summary>
    /// Tag yönetimi yarparken de sistemde tag varsa bu tag ataması yapılsın
    /// Eğer tag yoksa yeni bir tag tanımı yapılsın
    /// Taglerin önüne de # hash tag yapısı konsun.
    /// </summary>
    /// <param name="tagName"></param>
    public void AddTag(string tagName)
    {
      _tags.Add(new Tag(tagName));

    }

    /// <summary>
    /// Kategori sistemde tanımlı olanda olabilir olmayanda olabilir. Bu sebep ile ilgili categoryId ya yeni bir kategori sisteme girip ona verilen Id üzerinden yönetilsin. Yada var olan dbdeki categoryId bulunarak yönetilsin.
    /// </summary>
    /// <param name="categoryId"></param>
    /// <exception cref="Exception"></exception>
    public void SetCategory(string categoryId)
    {
      if (string.IsNullOrEmpty(categoryId))
      {
        throw new Exception("KategoriId alanı atanmalıdır");
      }

      CategoryId = categoryId;
    }




  }
}
