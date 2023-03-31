using BlogApp.Domain.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Entities
{
  // yani çoka çok ilişki kurduğumuz için EF araya bir post tag tablosu açacaktır.
  // TagId, PostId
  public class Tag : BaseEntity<int>
  {
    // Default constraint ile int Id alanları identity(1,1) default tanımlanır.
    public string Name { get; private set; }

    // 1 Tag 1 den fazla post ile alakalı olabilir
    public IReadOnlyCollection<Post> Posts { get; set; }


    public Tag(string name)
    {
      Name = name;
    }

  }
}
