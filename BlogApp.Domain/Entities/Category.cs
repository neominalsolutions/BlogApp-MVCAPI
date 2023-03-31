using BlogApp.Domain.SeedWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Entities
{
  public class Category:BaseEntity<string>
  {
    public string Name { get; private set; }

    public List<Post> _posts = new List<Post>();
    public IReadOnlyCollection<Post> Posts => _posts;


    public Category(string name)
    {
      Name = name;
      Id = Guid.NewGuid().ToString();
    }

  }
}
